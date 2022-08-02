using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPlayer : MonoBehaviour,ISaveable
{
    [SerializeField] bool played=false;

    public object CaptureState()
    {
        return played;
    }

    public void RestoreState(object state)
    {
        played = (bool) state;
    }

    public void SetPlayed(bool played)
    {
        this.played = played;
    }
    public bool IsIntroPlayed()
    {
        return played;
    }
}
