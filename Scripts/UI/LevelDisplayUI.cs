using TMPro;
using UnityEngine;

// Displays current level of the player in UI
public class LevelDisplayUI : MonoBehaviour
{
    [SerializeField] CharacterStats characterStat;
    [SerializeField] TextMeshProUGUI levelText;
    void Awake()
    {
        characterStat = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
    }
    void Update()
    {
        levelText.text = "LEVEL: "+characterStat.CurrentLevel().ToString();
    }
}
