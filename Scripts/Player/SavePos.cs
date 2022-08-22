using UnityEngine;

public class SavePos : MonoBehaviour, ISaveable
{
    [SerializeField] Vector3 pos;
    [SerializeField] Transform[] spawnPoint=new Transform[1];

    // Saving position of player
    object ISaveable.CaptureState()
    {
        return new SerializableVector3(transform.position);
    }
    // Loading position of player
    void ISaveable.RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        transform.position = position.ToVector();
    }

    void Start()
    {
        // Setting player spwn pos in different scenes depending on if day has started
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
