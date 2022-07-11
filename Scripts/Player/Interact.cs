using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.GetComponent<DialogueTrigger>())
                {
                    DialogueTrigger person = hit.transform.GetComponent<DialogueTrigger>();
                    transform.LookAt(new Vector3(person.transform.position.x, transform.position.y, person.transform.position.z));
                    if (Distance(person.gameObject) <= person.GetTalkingRadius())
                    {
                        dialogueBox.SetActive(true);
                        person.TriggerDialogue();
                    }
                }
                if (hit.transform.GetComponent<Pickup>())
                {
                    Pickup item = hit.transform.GetComponent<Pickup>();
                    transform.LookAt(new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z));
                    if (Distance(item.gameObject) <= item.GetPickupRadius())
                        item.PickupItem();
                }
            }
        }
    }
    float Distance(GameObject go)
    {
        return Vector3.Distance(transform.position, go.transform.position);
    }
}
