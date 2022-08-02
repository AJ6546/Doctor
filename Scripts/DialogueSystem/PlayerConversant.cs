using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConversant : MonoBehaviour
{
    [SerializeField] string playerName;

    Dialogue currentDialogue;
    [SerializeField] Dialogue testDialogue;
    DialogueNode currentNode=null;
    AIConversant currentConversant = null;
    bool isChoosing = false;

    public event Action onConversationUpdated;
   
    public void StartDialogue(AIConversant newConversant,Dialogue newDialogue)
    {
        currentConversant = newConversant;
        currentDialogue = newDialogue;
        currentNode = currentDialogue.GetRootNode();
        TriggerEnterAction();
        onConversationUpdated();
    }

    public bool IsActive()
    {
        return currentDialogue != null;
    }
    public bool IsChoosing()
    {
        return isChoosing;
    }

    public string GetText()
    {
        if(currentNode == null)
        {
            return "";
        }
        return currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return currentDialogue.GetPlayerChildren(currentNode);
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        Next();
    }

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

    public void Quit()
    {
        currentDialogue = null;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        currentConversant = null;
        onConversationUpdated();
    }
}
