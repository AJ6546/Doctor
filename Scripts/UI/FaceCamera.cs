using UnityEngine;

// used to make the object this script is attached to, to always face the camera
public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
