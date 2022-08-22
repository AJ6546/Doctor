using TMPro;
using UnityEngine;

public class RunTest : MonoBehaviour
{
    [SerializeField] Disease disease; // disease the patient has
    [SerializeField] GameObject runTestUI; // pop up to run tests
    GameObject player;
    [SerializeField] string action = "RunTest";
    [SerializeField] TextMeshProUGUI testResult;
   
    public void Start()
    {
        FindObjectOfType<PlayerConversant>().SetTalking(false);
        runTestUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        disease = player.GetComponent<Disease>();
    }
    public void SetActive(GameObject triggerObject)
    {
        if (triggerObject.GetComponent<DialogueTrigger>().ActionToTrigger() != action) return;
        runTestUI.SetActive(true);
        // The below cannot be used while running a test
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Shrink>().enabled = false;
        // setting isTalking to true

        player.GetComponent<PlayerConversant>().SetTalking(true);
    }
    public void OnButtonPressed(string test)
    {
        if (player.GetComponent<ClueCollector>().AllCluesCollected())
        {
            int levelToRunTest = disease.GetLevelToRunTest(test.ToUpper());
            // checking if the player is at required level
            if (player.GetComponent<CharacterStats>().CurrentLevel() >= levelToRunTest)
                testResult.text = disease.GetResult(test.ToUpper());
            else
                testResult.text = "You have not reached the level required to perform this test." +
                    "Try killing more invaders. You need to be at level " + levelToRunTest + " to be able to perform" +
                    "this test.";
        }
        // Displayed when player has not collected all clues
        else
            testResult.text = "It Seems like you haven't collected all the clues yet. " +
                "You need to collect all the clues before you can do a test";
    }
    // closing run test popup
    public void Quit()
    {
        // Enabled after run test is done
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Shrink>().enabled = true;
        // closing run test popup
        runTestUI.SetActive(false);
        // setting isTalking to false
        player.GetComponent<PlayerConversant>().SetTalking(false);
    }
}
