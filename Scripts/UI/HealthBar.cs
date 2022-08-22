using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health healthComponent;
    [SerializeField] RectTransform foreground;
    [SerializeField] Canvas rootCanvas = null;
    void Start()
    {
        healthComponent = GetComponentInParent<Health>();
    }

    void Update()
    {
        // do not display healthbar when health is full, health is 0 or player is dead
        if (healthComponent.IsDead()||Mathf.Approximately(healthComponent.GetHealthFraction(), 0) || Mathf.Approximately(healthComponent.GetHealthFraction(), 1))
        {
            rootCanvas.enabled = false;
            return;
        }
        rootCanvas.enabled = true;
        // Update health bar fg according to character's current health
        foreground.localScale = new Vector3(Mathf.Max(healthComponent.GetHealthFraction(), 0), 1, 1);
    }
}
