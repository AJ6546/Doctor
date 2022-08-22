using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    PlayerConversant playerConversant;
    [SerializeField] TextMeshProUGUI AIText;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject AIResponse;
    [SerializeField] Transform choiceRoot;
    [SerializeField] GameObject choicePrefab;
    [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI conversantName;

    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.onConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(() => playerConversant.Next());
        quitButton.onClick.AddListener(() => playerConversant.Quit());
        UpdateUI();
    }

    // Update the UI dialogue pop up
    private void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
            return;

        conversantName.text = playerConversant.GetCurrentConversantName(); // setting current coversant 
        AIResponse.SetActive(!playerConversant.IsChoosing()); // AI response is hidden when player is choosing
        choiceRoot.gameObject.SetActive(playerConversant.IsChoosing()); // choices are shown when playeris chooseing

        if(playerConversant.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            AIText.text = playerConversant.GetText(); // Setting dialoge
            nextButton.gameObject.SetActive(playerConversant.HasNext()); // showing a next button
        }
        
    }


    // Building a chooice list with the different options player can chose from
    private void BuildChoiceList()
    {
        foreach (Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }
        foreach (DialogueNode choice in playerConversant.GetChoices())
        {
            GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
            var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => 
            {
                playerConversant.SelectChoice(choice);
            });
        }
    }
}
