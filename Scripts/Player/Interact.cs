using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox; // pop up window to display on interaction with AI
    [SerializeField] AudioManager audioManager; // Instance of audio manager
    [SerializeField] string sfx; // SFX on interaction
    private void Start()
    {
        audioManager = AudioManager.instance;
    }
    void Update()
    {
        // getting ray from camera in direction og mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;
        // if the ray had hit anything
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // if the object ray had hit is has a AI Conversant attached, start a conversation
                if (hit.transform.GetComponent<AIConversant>())
                {
                    AIConversant person = hit.transform.GetComponent<AIConversant>();
                    transform.LookAt(new Vector3(person.transform.position.x, transform.position.y, person.transform.position.z));
                   
                    // Checking if player is within talking distance
                    if (Distance(person.gameObject) <= person.GetTalkingRadius() && !GetComponent<PlayerConversant>().IsTalking())
                    {
                        Dialogue dialogue = person.GetDialogue();
                        if (dialogue != null)
                        {
                            // play an sfx before starting conversation
                            sfx = "Hello";
                            audioManager.Play(sfx, transform.position);
                            //starting a conversation
                            GetComponent<PlayerConversant>().StartDialogue(person, dialogue);
                        }
                    }
                }
                // if the object ray had hit is has a Pickup attached, send it to inventory
                if (hit.transform.GetComponent<Pickup>())
                {
                    Pickup item = hit.transform.GetComponent<Pickup>();
                    transform.LookAt(new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z));
                    // Checking if player is within pickup distance
                    if (Distance(item.gameObject) <= item.GetPickupRadius())
                    {
                        // play an sfx for picking up object
                        sfx = "Pickup";
                        audioManager.Play(sfx, transform.position);
                        // Pick the object up and add it to inventory
                        item.PickupItem();
                    }
                }
                // if the object ray had hit is has a Door attached, Open it
                if (hit.transform.GetComponent<Door>())
                {
                    Door door = hit.transform.GetComponent<Door>();
                    transform.LookAt(new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z));
                    // Checking if player is within opening distance
                    if (Distance(door.gameObject) <= door.DoorRadius())
                    {
                        // play an sfx for opening the door
                        sfx = "Door";
                        audioManager.Play(sfx, transform.position);
                        // Open the door
                        door.Open();
                    }
                }
                // if the object ray had hit is has a TV attached, 
                // Load scene with build index=2 and run the gameplay video
                if (hit.transform.GetComponent<TV>())
                {
                    TV tv = hit.transform.GetComponent<TV>();
                    transform.LookAt(new Vector3(tv.transform.position.x, transform.position.y, tv.transform.position.z));
                    FindObjectOfType<Portal>().LoadSceneByIndex(2);
                }
            }
        }
    }
    float Distance(GameObject go)
    {
        return Vector3.Distance(transform.position, go.transform.position);
    }
}
