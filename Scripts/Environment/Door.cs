using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float doorRadius = 10f;
    [SerializeField] float doorWaitTime=2f;

    // sets door to inactive
     public void Open()
    {
        StartCoroutine(Close(doorWaitTime));
        door.SetActive(false);

    }

    // sets door to active with a delay
   IEnumerator Close(float time)
    {
        yield return new WaitForSeconds(time);
        door.SetActive(true); ;
    }

    internal float DoorRadius()
    {
        return doorRadius;
    }

    // Visualizing the radius within which door can be opened from
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, doorRadius);
    }
}
