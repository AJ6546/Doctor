using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] AudioManager audioManager;
    [SerializeField] string sfx;
    private void Start()
    {
        audioManager = AudioManager.instance;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.GetComponent<AIConversant>())
                {
                    AIConversant person = hit.transform.GetComponent<AIConversant>();
                    transform.LookAt(new Vector3(person.transform.position.x, transform.position.y, person.transform.position.z));
                   
                    if (Distance(person.gameObject) <= person.GetTalkingRadius())
                    {
                        Dialogue dialogue = person.GetDialogue();
                        if (dialogue != null)
                        {
                            sfx = "Hello";
                            audioManager.Play(sfx, transform.position);
                            GetComponent<PlayerConversant>().StartDialogue(person, dialogue);
                        }
                    }
                }
                if (hit.transform.GetComponent<Pickup>())
                {
                    Pickup item = hit.transform.GetComponent<Pickup>();
                    transform.LookAt(new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z));
                    if (Distance(item.gameObject) <= item.GetPickupRadius())
                    {
                        sfx = "Pickup";
                        audioManager.Play(sfx, transform.position);
                        item.PickupItem();
                    }
                }
                if (hit.transform.GetComponent<Door>())
                {
                    Door door = hit.transform.GetComponent<Door>();
                    transform.LookAt(new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z));
                    if (Distance(door.gameObject) <= door.DoorRadius())
                    {
                        sfx = "Door";
                        audioManager.Play(sfx, transform.position);
                        door.Open();
                    }
                }
            }
        }
    }
    float Distance(GameObject go)
    {
        return Vector3.Distance(transform.position, go.transform.position);
    }
}
