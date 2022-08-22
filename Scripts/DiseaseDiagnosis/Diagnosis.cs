using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diagnosis : MonoBehaviour
{
    [SerializeField] Disease disease; // Disease patient has
    [SerializeField] Text enteredDisease; // Disease player entered
    [SerializeField] TextMeshProUGUI diseaseDescription; // Descriptionof disease patient has
    [SerializeField] GameObject diagnosisUI; // Pop up for player to enter disease
    GameObject player;
    [SerializeField] string action = "Diagnosis";
    [SerializeField] bool diagnosed=false;
    public void Start()
    {
        diagnosisUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        disease = player.GetComponent<Disease>();
    }
    public void SetActive(GameObject triggerObject)
    {
        if (triggerObject.GetComponent<DialogueTrigger>().ActionToTrigger() != action) return;
        diagnosisUI.SetActive(true);

        // The below cannot be used while diagnosing a disease
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Shrink>().enabled = false;
        // setting isTalking to true
        player.GetComponent<PlayerConversant>().SetTalking(true);
    }

    // Pressing Button after entering disease.
    public void OnEnterDiseasePressed()
    {
        if (enteredDisease.text == "") return;
        string diseaseEntered = enteredDisease.text.ToUpper(); // All to upper case
        // If plyer enters disease without collecting all clues, we display a message,
        // asking playerto collect all clues
        if (!player.GetComponent<ClueCollector>().AllCluesCollected())
        {
            diseaseDescription.text = "You must collect more clues before proceeding";
        }
        else
        {
            // Diagnosis is wrong
            if (diseaseEntered != disease.GetDisease().ToUpper())
            {
                StartCoroutine(Diagnose(false));
            }
            // Diagnosis is right
            else
            {
                StartCoroutine(Diagnose(true));
            }
        }
    }
    IEnumerator Diagnose(bool correct)
    {
        // Getting readto start next round
        player.GetComponent<ItemManager>().UnEquip(2); // remove any weapon item
        player.GetComponent<PlayerInventory>().RemoveAllItems(); // remove all items from inventory
        player.GetComponent<ClueCollector>().RemoveCollectedClues(); // remove all clues collected
        player.GetComponent<Achiever>().UpdateTotalDiseasesDiagnosed(); // update tatal diseases diagnosed count
        diagnosed = true;

        // when correct, displaye a message, gain experience and update diseases diagnosed correctly
        if (correct)
        {
            diseaseDescription.text = "Your diagnosis was correct. You will gain some experience";
            player.GetComponent<Experience>().GainExperience(disease.GetDiseaseExperience());
            player.GetComponent<Achiever>().UpdateDiagnoseSeucceeded();
        }
        // when incorrect, display message and lose experience
        else
        {
            diseaseDescription.text = "You diagnosed incorrectly. You will lose some experience";
            player.GetComponent<Experience>().GainExperience(-disease.GetDiseaseExperience());
        }
        // display disease description after delay
        yield return new WaitForSeconds(2f);
        diseaseDescription.text = disease.GetDescription();
    }

    // closing diagnosis popup
    public void Quit()
    {
        // Player  gets a new disease
        player.GetComponent<Disease>().UpdateDisease();
        // Enabled after diagnosis is done
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Shrink>().enabled = true;

        // closing diagnosis ui
        diagnosisUI.SetActive(false);

        // new disease is undiagnosed.
        if (diagnosed == true)
        {
            // setting day starter to false
            player.GetComponent<DayStarter>().ResetDay(false);
            // loading same scene
            FindObjectOfType<Portal>().LoadCurrentScene();
        }
        // setting isTalking to false
        player.GetComponent<PlayerConversant>().SetTalking(false);
    }
}
