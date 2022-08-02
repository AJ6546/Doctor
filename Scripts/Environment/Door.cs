using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float doorRadius = 10f;
    [SerializeField] float doorWaitTime=2f;
     public void Open()
    {
        StartCoroutine(Close(doorWaitTime));
        door.SetActive(false);

    }

   IEnumerator Close(float time)
    {
        yield return new WaitForSeconds(time);
        door.SetActive(true); ;
    }

    internal float DoorRadius()
    {
        return doorRadius;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, doorRadius);
    }
}
