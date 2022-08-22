using TMPro;
using UnityEngine;

// Displays a messge with current date at the begining of each new round
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
