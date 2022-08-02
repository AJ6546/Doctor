using System.Collections;
using TMPro;
using UnityEngine;

public class DayStarterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dayStarterText;
    [SerializeField] DayStarter dayStarter;
    void Awake()
    {
        dayStarter = GameObject.FindGameObjectWithTag("Player").GetComponent<DayStarter>();
    }
    void Update()
    {
        if (!dayStarter.HasDayStarted())
        { dayStarterText.text = System.DateTime.Today.ToString("MMMM dd, yyyy") + "\nAnother day begins"; }
        else
        {
            gameObject.SetActive(false);
        }
    }
   
}
