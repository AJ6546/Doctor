using UnityEngine;

public class SavePos : MonoBehaviour, ISaveable
{
    [SerializeField] Vector3 pos;
    [SerializeField] Transform spawnPoint;
    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        transform.position = position.ToVector();
    }

    void Start()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
