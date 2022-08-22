using UnityEngine;

public class OnlineAuthenticator : MonoBehaviour,ISaveable
{
    [SerializeField] bool isRegistered;
    [SerializeField] GameObject onlineSaveButton, onlineLoadButton, onlineRegisterButton;
    void Start()
    {
    }

    void Update()
    {
        // If the player is registered show save and load buttons
        // Hide Register button
        if (isRegistered)
        {
            onlineRegisterButton.SetActive(false);
            onlineSaveButton.SetActive(true);
            onlineLoadButton.SetActive(true);
        }
        // If player is not registered show register button
        // Hide save and load button
        else
        {
            onlineRegisterButton.SetActive(true);
            onlineSaveButton.SetActive(false);
            onlineLoadButton.SetActive(false);
        }
    }
    // Getter and setter
    public bool IsRegistered()
    {
        return isRegistered;
    }
    void SetRegistered(bool isRegistered)
    {
        this.isRegistered = isRegistered;
    }

    public void Regisetered()
    {
        SetRegistered(true);
    }


    // Saving if the player is registered or not
    object ISaveable.CaptureState()
    {
        return isRegistered;
    }

    // Loading if the player is registered or not
    void ISaveable.RestoreState(object state)
    {
        isRegistered =(bool) state;
    }
}
