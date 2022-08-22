using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConversant : MonoBehaviour
{
    [SerializeField] string playerName;

    Dialogue currentDialogue;
    DialogueNode currentNode=null;
    AIConversant currentConversant = null;
    bool isChoosing = false;
    [SerializeField]  bool isTalking=false;

    public event Action onConversationUpdated;
   
    // Starts a dialoge
    public void StartDialogue(AIConversant newConversant,Dialogue newDialogue)
    {
        currentConversant = newConversant; // sets up the conversant
        currentDialogue = newDialogue; // sets up dialogue
        currentNode = currentDialogue.GetRootNode(); // sets up dialogue node
        TriggerEnterAction(); // triggers enter action if any
        onConversationUpdated(); // Used to update ui 
        isTalking=true; // sets isTalking to restrict action while in conversation
    }

    // returns true if there is a dialogue and false otherwise
    public bool IsActive()
    {
        return currentDialogue != null;
    }

    // returns true if player is choosing from options
    public bool IsChoosing()
    {
        return isChoosing;
    }

    // returns the text in current node.
    public string GetText()
    {
        if(currentNode == null)
        {
            return "";
        }
        return currentNode.GetText();
    }

    // All options the player can choose from
    public IEnumerable<DialogueNode> GetChoices()
    {
        return currentDialogue.GetPlayerChildren(currentNode);
    }

    // Takes the conversation depending on palyer's choice
    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        Next();
    }

    // Name of person talking to be displayed in ui
    public string GetCurrentConversantName()
    {
        if(IsChoosing())
        {
            return playerName;
        }
        else
        {
            return currentConversant.GetConversantName();
        }
    }

    public void Next()
    {
        int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
        // If there are more than 1 responses isChoosing is set true
        if(numPlayerResponses>0)
        {
            isChoosing = true;
            TriggerExitAction();
            onConversationUpdated();
            return;
        }
        DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
        int randomIndex = UnityEngine.Random.Range(0, children.Count());
        TriggerExitAction();
        currentNode= children[randomIndex];
        TriggerEnterAction();
        onConversationUpdated();
    }
    public bool HasNext()
    {
        return currentDialogue.GetAllChildren(currentNode).Count() > 0;
    }

    void TriggerEnterAction()
    {
        if(currentNode!=null)
        {
            TriggerAction(currentNode.GetOnEnterAction());
        }
    }

    void TriggerExitAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnExitAction());
        }
    }

    void TriggerAction(string action)
    {
        if (action == "") return;
        foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
        {
            trigger.Trigger(action);
        }
    }

    // Quitting a conversation
    public void Quit()
    {
        currentDialogue = null;
        isTalking = false;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        currentConversant = null;
        onConversationUpdated();
    }

    // Getting and setting isTaking.
    public bool IsTalking()
    {
        return isTalking;
    }
    public void SetTalking(bool isTalking)
    {
        this.isTalking = isTalking;
    }
}
