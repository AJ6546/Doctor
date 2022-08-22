using UnityEngine;

// The game intro needs to be played only once, when the player starts the game for the first time
// After the first save, We assume the player already kows what to do and stop bothering with the intro
// every time the game loads
public class IntroPlayer : MonoBehaviour,ISaveable
{
    [SerializeField] bool played=false;

    // Saving if intro has been played or not
    public object CaptureState()
    {
        return played;
    }

    // Loading if intro has been played or not
    public void RestoreState(object state)
    {
        played = (bool) state;
    }

    // Getter and Setter
    public bool IsIntroPlayed()
    {
        return played;
    }
    public void SetPlayed(bool played)
    {
        this.played = played;
    }
    
}
