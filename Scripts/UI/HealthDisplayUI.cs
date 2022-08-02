using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
