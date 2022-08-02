using TMPro;
using UnityEngine;


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
