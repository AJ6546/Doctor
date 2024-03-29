using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DungeonStates{
    inactive,
    generatingMain,
    generatingBranches,
    cleanUp,
    completed
};
public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] startPrefabs;
    [SerializeField] GameObject[] tilePrefabs;
    [SerializeField] GameObject[] blockedPrefabs;

    [Header("Debugging Options")]
    [SerializeField] bool useBoxColliders;
    [SerializeField] bool useLightsForDebugging;
    [SerializeField] bool restoreLightsAfterDebugging;

    [Header("Keybindings")]
    [SerializeField] KeyCode reloadKey = KeyCode.Backspace;
    [SerializeField] KeyCode toggleMapkey = KeyCode.M;

    [Header("Generation Limits")]
    [Range(2,100)] [SerializeField] int mainLength=10;
    [Range(0, 50)] [SerializeField] int branchLength=5;
    [Range(0, 25)] [SerializeField] int numBranches= 10;
    [Range(0, 100)] [SerializeField] int doorPercent = 25;
    [Range(0, 1f)] [SerializeField] float constructionDelay;

    [Header("Available Runtimes")]
    [SerializeField] List<Tile> generatedTiles = new List<Tile>();

    [HideInInspector] public DungeonStates dungeonState = DungeonStates.inactive;

    List<Connecter> availableConnectors = new List<Connecter>();
    Color startLightColor = Color.white;
    Transform tileFrom, tileTo, tileRoot;
    Transform container;
    int attempts,maxAttempts=50;

    void Start()
    {
        StartCoroutine(DungeonBuild());
    }

    // Build dungeon
    IEnumerator DungeonBuild()
    {
        GameObject goContainer = new GameObject("Main Path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = CreateStartTile(); // Starting tile is special and has only 1 connector
        DebugRoomLighting(tileRoot, Color.blue); // set lighting for the room
        tileTo = tileRoot; // tileTo is now starting room
        dungeonState = DungeonStates.generatingMain; // stting state 
        for (int i = 0; i < mainLength-1; i++)
        {
            yield return new WaitForSeconds(constructionDelay);
            tileFrom = tileTo; // In the begining tile from will be starting room
            tileTo = CreateTile();
            DebugRoomLighting(tileTo, Color.red);
            ConnectTiles();
            CollisionCheck();
            if (attempts >= maxAttempts)
            {
                break;
            }
        }
        // Get all connectors within container that are NOT already connected
        foreach(Connecter connector in container.GetComponentsInChildren<Connecter>())
        {
            if(!connector.IsConnected())
            {
                if(!availableConnectors.Contains(connector))
                    availableConnectors.Add(connector);
            }
        }
        // Branching
        dungeonState = DungeonStates.generatingBranches;
        for (int b = 0; b < numBranches; b++) // looping over number of branches
        {
            if (availableConnectors.Count > 0)
            {
                goContainer = new GameObject("Branch " + (b + 1)); // naming branches
                container = goContainer.transform;
                container.SetParent(transform);
                int availIndex = Random.Range(0, availableConnectors.Count);
                tileRoot = availableConnectors[availIndex].transform.parent.parent;
                availableConnectors.RemoveAt(availIndex);
                tileTo = tileRoot;
                for (int i = 0; i < branchLength - 1; i++) // looping through branch length for each branch
                {
                    yield return new WaitForSeconds(constructionDelay);
                    tileFrom = tileTo;
                    tileTo = CreateTile(); // creating new room
                    DebugRoomLighting(tileTo, Color.green);
                    // connecting rooms
                    ConnectTiles();
                    CollisionCheck();
                    if (attempts >= maxAttempts)
                    {
                        break;
                    }
                }
            }
            else { break; }
        }
        // stting dungeon state
        dungeonState = DungeonStates.cleanUp;
        LightRestoration();
        CleanUpBoxes();
        BlockPassages();
        // stting dungeon state
        dungeonState = DungeonStates.completed;
        yield return null;
    }

    // Blocking unconnected connectors
    private void BlockPassages()
    {
        foreach(Connecter connecter in  transform.GetComponentsInChildren<Connecter>())
        {
            if(!connecter.IsConnected())
            {
                Vector3 pos = connecter.transform.position;
                int wallIndex = Random.Range(0, blockedPrefabs.Length);
                GameObject goWall = Instantiate(blockedPrefabs[wallIndex], pos, connecter.transform.rotation, 
                    connecter.transform) as GameObject;
                goWall.name = blockedPrefabs[wallIndex].name;

            }
        }
    }

    Transform CreateStartTile()
    {
        int index = Random.Range(0, startPrefabs.Length);
        // Instantiating the starting room
        GameObject goTile = Instantiate(startPrefabs[index], Vector3.zero, 
            Quaternion.identity, container) as GameObject;
        goTile.name = "Start Room";
        // Setting the y rotation of the room to be 0, 90, 270 or 360
        float yRot = Random.Range(0, 4) * 90f;
        goTile.transform.Rotate(0, yRot, 0);
        // Add to generatedTiles
        generatedTiles.Add(new Tile(goTile.transform, null));
        return goTile.transform;
    }

    Transform CreateTile()
    {
        int index = Random.Range(0, tilePrefabs.Length);
        // Instantiate a new room
        GameObject goTile = Instantiate(tilePrefabs[index], 
            Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = tilePrefabs[index].name;
        // Add to generatedTiles
        Transform origin = generatedTiles[generatedTiles.
            FindIndex(x => x.tile == tileFrom)].tile;
        // Adding to generated tiles list
        generatedTiles.Add(new Tile(goTile.transform, origin));
        return goTile.transform;
    }

    void ConnectTiles()
    {
        // checking for unconnected connectors to and from tiles
        Transform connectFrom = GetRandomConnector(tileFrom);
        if (connectFrom == null) return;
        Transform connectTo = GetRandomConnector(tileTo);
        if (connectTo == null) return;

        // connecting
        connectTo.SetParent(connectFrom);
        tileTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero;
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0,180f,0);
        tileTo.SetParent(container);
        connectTo.SetParent(tileTo.Find("Connectors"));
        generatedTiles.Last().connector = connectFrom.GetComponent<Connecter>();
    }

    Transform GetRandomConnector(Transform tile)
    {
        if(tile==null) return null;

        // find all unconnected connectors of the tile
        List<Connecter> connectorList = tile.GetComponentsInChildren<Connecter>().ToList().
            FindAll(x=>x.IsConnected()==false);
        if(connectorList.Count>0)
        {
            //select a random connector from the tile
            int connecterIndex = Random.Range(0, connectorList.Count);
            //setting isConnected on that connector
            connectorList[connecterIndex].SetConnected(true);
            if(tile==tileFrom)
            {
                BoxCollider box = GetComponent<BoxCollider>();
                // creating box collider and setting isTrigger if the room do not already have one
                if(box==null)
                {
                    box = tile.gameObject.AddComponent<BoxCollider>();
                    box.isTrigger = true;
                }
            }
            return connectorList[connecterIndex].transform;
        }
        return null;
    }

    // Destroy box colliders after dungeon is built.
    void CleanUpBoxes()
    {
        if(!useBoxColliders)
        {
            foreach(Tile myTile in generatedTiles)
            {
                BoxCollider box = myTile.tile.GetComponent<BoxCollider>();
                if(box!=null) { Destroy(box); }
            }
        }
    }
    void CollisionCheck()
    {
        BoxCollider box = tileTo.GetComponent<BoxCollider>();
        if(box==null)
        {
            box = tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }
        Vector3 offset = (tileTo.right * box.center.x) + (tileTo.up * box.center.y) + (tileTo.forward * box.center.z);
        Vector3 halfExtends = box.bounds.extents;
        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, halfExtends, Quaternion.identity,
            LayerMask.GetMask("Tile")).ToList();
        if(hits.Count>0)
        {
            if(hits.Exists(x=>x.transform != tileFrom  && x.transform != tileTo))
            {
                // Hit something other than tileFrom and tileTo.
                attempts++;
                int toIndex = generatedTiles.FindIndex(x => x.tile == tileTo);
                if(generatedTiles[toIndex].connector!=null)
                {
                    generatedTiles[toIndex].connector.SetConnected(false);
                }
                generatedTiles.RemoveAt(toIndex);
                DestroyImmediate(tileTo.gameObject);
                // Backtracking
                if(attempts>=maxAttempts)
                {
                    int fromIndex = generatedTiles.FindIndex(x => x.tile == tileFrom);
                    Tile myTileFrom = generatedTiles[fromIndex];
                    if(tileFrom!=tileRoot)
                    {
                        if(myTileFrom.connector!=null)
                        {
                            myTileFrom.connector.SetConnected(false);
                        }
                        availableConnectors.RemoveAll(x => x.transform.parent.parent=tileFrom);
                        generatedTiles.RemoveAt(fromIndex);
                        DestroyImmediate(tileFrom.gameObject);
                        if(myTileFrom.origin!=tileRoot)
                        {
                            tileFrom = myTileFrom.origin;
                        }
                        else if (container.name.Contains("Main"))
                        {
                            if (myTileFrom.origin != null)
                            {
                                tileRoot = myTileFrom.origin;
                                tileFrom = tileRoot;
                            }
                        }
                        else if (availableConnectors.Count > 0)
                        {
                            int availIndex = Random.Range(0, availableConnectors.Count);
                            tileRoot = availableConnectors[availIndex].transform.parent.parent;
                            availableConnectors.RemoveAt(availIndex);
                            tileFrom = tileRoot;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if(container.name.Contains("Main"))
                    {
                        if(myTileFrom.origin!=null)
                        {
                            tileRoot = myTileFrom.origin;
                            tileFrom = tileRoot;
                        }
                    }
                    else if(availableConnectors.Count>0)
                    {
                        int availIndex = Random.Range(0, availableConnectors.Count);
                        tileRoot = availableConnectors[availIndex].transform.parent.parent;
                        availableConnectors.RemoveAt(availIndex);
                        tileFrom = tileRoot;
                    }
                    else
                    {
                        return;
                    }
                }
                // Retry
                if (tileFrom != null)
                {
                    tileTo = CreateTile();
                    Color retryColor = container.name.Contains("Branch") ? Color.yellow : Color.green;
                    DebugRoomLighting(tileTo, retryColor * 2f);
                    ConnectTiles();
                    CollisionCheck();
                }
            }
            else
            {
                attempts = 0; // Nothing other than tileFrom and tileTo were hit (So restore attemps back to 0).
            }
        }
    }


    void DebugRoomLighting(Transform tile, Color lightColor)
    {
        if(useLightsForDebugging && Application.isEditor)
        {
            Light[] lights = tile.GetComponentsInChildren<Light>();
            if(startLightColor==Color.white)
            {
                startLightColor = lights[0].color;
            }
            foreach(Light light in lights)
            {
                light.color = lightColor;
            }
        }
    }

    void LightRestoration()
    {
        if(useLightsForDebugging && restoreLightsAfterDebugging && Application.isEditor)
        {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach( Light light in lights)
            {
                light.color = startLightColor;
            }
        }
    }

    void Update()
    {
        // If Player is talking, Do not do below
        if (FindObjectOfType<PlayerConversant>().IsTalking()) return;
        // If Player is Saving the game online Do not do below
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;
        // Reload the same scene
        if (Input.GetKeyDown(reloadKey))
        {
            FindObjectOfType<Portal>().LoadCurrentScene();
        }
    }
}
