using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePos : MonoBehaviour, ISaveable
{
    [SerializeField] Vector3 pos;
    [SerializeField] Transform[] spawnPoint=new Transform[1];

    object ISaveable.CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    void ISaveable.RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        transform.position = position.ToVector();
    }

    void Start()
    {
        if (!GetComponent<DayStarter>().HasDayStarted())
        {
            transform.position = spawnPoint[1].position;
            transform.rotation = spawnPoint[1].rotation;
        }
        else
        {
            transform.position = spawnPoint[0].position;
            transform.rotation = spawnPoint[0].rotation;
        }
    }
}
