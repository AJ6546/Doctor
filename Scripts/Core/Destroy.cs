using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] float time=5f;
    [SerializeField] GameObject targetToDestroy = null;
    // Destroys the gameobject after the game has begun with a delay. default delay=5f
    void Start()
    {
        if(targetToDestroy!=null)
            Destroy(targetToDestroy, time);
        else
            Destroy(gameObject, time);
    }
}
