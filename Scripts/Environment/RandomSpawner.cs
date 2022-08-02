using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    DungeonGenerator myGenerator;
    bool isCompleted=false;
    [SerializeField] string[] spawnObjects = new string[3];
    PoolManager poolManager;
    [SerializeField] Transform[] spawnPoint= new Transform[3];
    void Start()
    {
        myGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        poolManager = PoolManager.instance;
    }

    void Update()
    {
        if (!isCompleted && myGenerator.dungeonState == DungeonStates.completed)
        {
            isCompleted = true;
            for(int i=0;i<spawnObjects.Length;i++)
            {
                if (Random.Range(1, 11) % 2 == 0)
                    poolManager.Spawn(spawnObjects[i], spawnPoint[i].position, transform, false);
            }
        }
    }
}
