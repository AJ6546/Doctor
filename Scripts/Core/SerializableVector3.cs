using UnityEngine;

[System.Serializable]
public class SerializableVector3
{
    // Used to serialize position (Vector 3) when saving
    float x, y, z;
    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3 ToVector()
    {
        return new Vector3(x,y,z);
    }
}
