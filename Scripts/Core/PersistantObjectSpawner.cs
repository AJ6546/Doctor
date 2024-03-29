using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab = null;

    // Used to have persist objects between scene
    static bool hasSpawned = false;
    private void Awake()
    {
        if (hasSpawned) return;

        SpawnPersistentObjects();

        hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
