using System.Collections;
using UnityEngine;

public class DayStarter : MonoBehaviour,ISaveable
{
    [SerializeField] bool dayStart=false;

    // Used to display a message with date before a new round begins
    // Also determines players position in hospital scene
    private void Update()
    {
        if (!dayStart && GetComponent<IntroPlayer>().IsIntroPlayed())
            StartCoroutine(StartDay());
    }

    IEnumerator StartDay()
    {
        yield return new WaitForSeconds(2f);
        dayStart = true;
    }
    public bool HasDayStarted()
    {
        return dayStart;
    }
    public void ResetDay(bool dayStart)
    {
        this.dayStart = dayStart;
    }

    // Saving if the day has already started or not
    public object CaptureState()
    {
        return dayStart;
    }

    // Loading if the day has already started or not
    public void RestoreState(object state)
    {
        dayStart = (bool)state;
    }
    
}
