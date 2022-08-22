using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // A Serializable class to add prefabs with audio effects and their corresponding tag
    // from the Unity Editor
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public string tag;
    }

    // Creating a Singleton of AudioManager
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, Queue<GameObject>> pooldictionary;
    [SerializeField] List<Pool> pools = new List<Pool>();
    void Start()
    {
        pooldictionary = new Dictionary<string, Queue<GameObject>>();
        // finding different prefabs in the pool and instatiating them at begining of the scene
        // Setting them to inactive for use in future.
        // Adding them to the pool dictionary
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            GameObject prefab = Instantiate(pool.prefab, transform.position, pool.prefab.transform.rotation);
            ActivatePrefab(ref prefab);
            objPool.Enqueue(prefab);
            pooldictionary.Add(pool.tag, objPool);
        }

    }
    void ActivatePrefab(ref GameObject prefab)
    {
        prefab.SetActive(false);
    }

    // Called to play the sound effect on Prefab
    public void Play(string tag, Vector3 pos)
    {
        GameObject obj = pooldictionary[tag].Dequeue(); // Dequeue the object with the tag
        obj.SetActive(true); // Set it to active
        obj.transform.position = pos; // set its position to where the sound needs to be played from
        if (obj.GetComponent<AudioSource>())
            obj.GetComponent<AudioSource>().Play(); // Playing the SFX
        pooldictionary[tag].Enqueue(obj); // Enqueueing it again for future use
    }
}

