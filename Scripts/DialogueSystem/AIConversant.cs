using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] float talkingRadius = 5f;
    [SerializeField] string conversantName;

    public float GetTalkingRadius()
    {
        return talkingRadius;
    }

    public Dialogue GetDialogue()
    {
        return dialogue;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, talkingRadius);
    }

    public string GetConversantName()
    {
        return conversantName;
    }
}
