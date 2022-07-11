using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, dialogueText;

    Queue<string> senetences;
    void Start()
    {
        senetences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        senetences.Clear();
        foreach(string sentence in dialogue.senetences)
        {
            senetences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(senetences.Count==0)
        {
            EndDialogue();
            return;
        }
        string sentence = senetences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {

    }
}
