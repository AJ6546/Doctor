using TMPro;
using UnityEngine;
// Displays Health in UI
public class HealthDisplayUI : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] TextMeshProUGUI levelText;
    void Awake()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    void Update()
    {
        levelText.text = "HEALTH: " + health.GetHealth()+"/"+health.GetStartHealth();
    }
}
