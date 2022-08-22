using System;
using UnityEngine;
using UnityEngine.UI;

// Displays damage te character takes over its head
public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    // sets text field to damage the character is taking
    public void SetDamageText(float damage)
    {
        damageText.text = Convert.ToString(damage);
    }
}
