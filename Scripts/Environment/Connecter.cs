using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connecter : MonoBehaviour
{
    public Vector2 size = Vector2.one * 4;
    [SerializeField] bool isConnected, isPlaying;
    private void Start()
    {
        isPlaying = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = isConnected ? Color.green : Color.red;
        if (!isPlaying)
        { Gizmos.color = Color.cyan; }
        Vector2 halfsize = size * 0.5f;
        Vector3 offset = transform.position + transform.up * halfsize.y;
        Gizmos.DrawLine(offset, offset + transform.forward);

        // Define top and side Vectors
        Vector3 top = transform.up * size.y;
        Vector3 side = transform.right * halfsize.x;
        // Define Corner Vectors
        Vector3 topRight = transform.position + top + side;
        Vector3 topLeft = transform.position + top - side;
        Vector3 botRight = transform.position + side;
        Vector3 botLeft = transform.position - side;

        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);

        // Draw Diagonal Lines
        Gizmos.color *= 0.7f;
        Gizmos.DrawLine(topRight, offset);
        Gizmos.DrawLine(topLeft, offset);
        Gizmos.DrawLine(botLeft, offset);
        Gizmos.DrawLine(botRight, offset);
    }

    public bool IsConnected()
    {
        return isConnected;
    }
    public void SetConnected(bool isConnected)
    {
        this.isConnected = isConnected;
    }
}
