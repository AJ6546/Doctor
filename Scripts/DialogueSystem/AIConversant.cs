using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] float talkingRadius = 5f;
    [SerializeField] string conversantName;

    // This is the radius within which player can talk with this AI
    public float GetTalkingRadius()
    {
        return talkingRadius;
    }

   

    // Visual representation of talking distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, talkingRadius);
    }

    // Getters
    public Dialogue GetDialogue()
    {
        return dialogue;
    }
    public string GetConversantName()
    {
        return conversantName;
    }
}
