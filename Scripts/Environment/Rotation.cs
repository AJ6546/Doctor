using UnityEngine;

// Used to rotate a game object in x,y,z directions. Used on pickups.
public class Rotation : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    [SerializeField] RotationAxis rotationAxis;
    Vector3 rotateDirection;
    private void Start()
    {
        if(rotationAxis==RotationAxis.x)
        { rotateDirection = Vector3.right; }
        else if (rotationAxis == RotationAxis.y)
        { rotateDirection = Vector3.up; }
        else
        { rotateDirection = Vector3.forward; }
    }
    private void Update()
    {
        transform.Rotate(rotateDirection * speed * Time.deltaTime);
    }
}
enum RotationAxis
{
    x,
    y,
    z
}
