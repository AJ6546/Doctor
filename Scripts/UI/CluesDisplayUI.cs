using TMPro;
using UnityEngine;

// Displays collected clues/ total clues in ui
public class CluesDisplayUI : MonoBehaviour
{
    [SerializeField] ClueCollector clueCollector;
    [SerializeField] TextMeshProUGUI cluesText;
    void Start()
    {
        clueCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<ClueCollector>();
    }
    void Update()
    {
        cluesText.text = "Clues: "+clueCollector.collectedClues.Count + "/" + clueCollector.clueItems.Count;
    }
}
