using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diagnosis : MonoBehaviour
{
    [SerializeField] Disease disease;
    [SerializeField] Text enteredDisease;
    [SerializeField] TextMeshProUGUI diseaseDescription;
    [SerializeField] GameObject diagnosisUI;
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
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Shrink>().enabled = false;
    }
    public void OnEnterDiseasePressed()
    {
        if (enteredDisease.text == "") return;
        string diseaseEntered = enteredDisease.text.ToUpper();
        if (!player.GetComponent<ClueCollector>().AllCluesCollected())
        {
            diseaseDescription.text = "You must collect more clues before proceeding";
        }
        else
        {
            if (diseaseEntered != disease.GetDisease().ToUpper())
            {
                StartCoroutine(Diagnose(false)); 
            }
            else
            {
                StartCoroutine(Diagnose(true));
            }
        }
    }
    IEnumerator Diagnose(bool correct)
    {
        player.GetComponent<ItemManager>().UnEquip(2);
        player.GetComponent<PlayerInventory>().RemoveAllItems();
        player.GetComponent<ClueCollector>().RemoveCollectedClues();
        diagnosed = true;
        if (correct)
        {
            diseaseDescription.text = "Your diagnosis was correct. You will gain some experience";
            player.GetComponent<Experience>().GainExperience(disease.GetDiseaseExperience());
        }
        else
        {
            diseaseDescription.text = "You diagnosed incorrectly. You will lose some experience";
            player.GetComponent<Experience>().GainExperience(-disease.GetDiseaseExperience());
        }
        yield return new WaitForSeconds(2f);
        diseaseDescription.text = disease.GetDescription();
    }
    public void Quit()
    {
        player.GetComponent<Disease>().UpdateDisease();
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Shrink>().enabled = true;
        diagnosisUI.SetActive(false);
        if (diagnosed == true)
        {
            player.GetComponent<DayStarter>().ResetDay(false);
            FindObjectOfType<Portal>().LoadCurrentScene();
        }
    }
}
