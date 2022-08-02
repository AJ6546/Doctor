using System.Collections;
using UnityEngine;

public class DayStarter : MonoBehaviour,ISaveable
{
    [SerializeField] bool dayStart=false;

    private void Update()
    {
        if (!dayStart && GetComponent<IntroPlayer>().IsIntroPlayed())
            StartCoroutine(StartDay());
    }
    public object CaptureState()
    {
        return dayStart;
    }

    public void RestoreState(object state)
    {
        dayStart = (bool)state;
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
}
