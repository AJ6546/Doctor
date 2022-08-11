using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] List<string> actions=new List<string>();
    [SerializeField] UnityEvent onTrigger;
    string actionToTrigger;
    public void Trigger(string actionToTrigger)
    {
        this.actionToTrigger = actionToTrigger;
        if (actions.Contains(actionToTrigger))
        {
            onTrigger.Invoke();  
        }
    }
    public string ActionToTrigger()
    {
        return actionToTrigger;
    }
}
