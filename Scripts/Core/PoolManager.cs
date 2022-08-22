using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // A Serializable class to add prefabs with audio effects and their corresponding tag
    // from the Unity Editor
    [System.Serializable]
    public class Pool
    {
        public int size; // Number of prefabs required
        public GameObject prefab;
        public string tag;
        public float x_max, y_max, z_max; // position to spawn the prefab
    }
    // Creating a Singleton of AudioManager
    public static PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, Queue<GameObject>> pooldictionary;
    [SerializeField] List<Pool> pools = new List<Pool>();
    void Start()
    {
        pooldictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            // finding different prefabs in the pool and instatiating them at begining of the scene
            // Setting them to inactive or active depending on the prefab's tag for use in future.
            // Pickup items must be active at the beginning of the game. VFX need not.
            // Adding them to the pool dictionary
            for (int i = 0; i < pool.size; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0, pool.x_max), pool.y_max, Random.Range(0, pool.z_max));
                GameObject prefab = Instantiate(pool.prefab, pos, pool.prefab.transform.rotation);
                ActivatePrefab(ref prefab);
                objPool.Enqueue(prefab);
            }
            pooldictionary.Add(pool.tag, objPool);
        }
    }
    public void Spawn(string tag, Vector3 pos, Transform instantiator, bool instantiatorRot = false)
    {
        GameObject obj = pooldictionary[tag].Dequeue(); // Dequeue the object with the tag
        obj.SetActive(true); // Set it to active
        obj.transform.position = pos; // set its position 
        if (instantiatorRot == true) // weateher the object should follow instantiator's rotation
            obj.transform.rotation = instantiator.rotation;
        if (obj.GetComponent<Deactivate>()) // Deactivate object after a delay if required.
            obj.GetComponent<Deactivate>().Disable();
        pooldictionary[tag].Enqueue(obj); // Enqueueing it again for future use
    }
    void ActivatePrefab(ref GameObject prefab)
    {
        switch (prefab.tag)
        {
            case "Pickup":
                prefab.SetActive(true);
                break;
            default:
                prefab.SetActive(false);
                break;
        }
    }
}
