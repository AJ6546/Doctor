using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineAuthenticator : MonoBehaviour,ISaveable
{
    [SerializeField] bool isRegistered;
    [SerializeField] GameObject onlineSaveButton, onlineLoadButton, onlineRegisterButton;
    void Start()
    {
    }

    void Update()
    {

        if (isRegistered)
        {
            onlineRegisterButton.SetActive(false);
            onlineSaveButton.SetActive(true);
            onlineLoadButton.SetActive(true);
        }
        else
        {
            onlineRegisterButton.SetActive(true);
            onlineSaveButton.SetActive(false);
            onlineLoadButton.SetActive(false);
        }
    }

    public bool IsRegistered()
    {
        return isRegistered;
    }
    public void SetRegistered(bool isRegistered)
    {
        this.isRegistered = isRegistered;
    }

    public void Regisetered()
    {
        SetRegistered(true);
        onlineRegisterButton.SetActive(false);
        onlineSaveButton.SetActive(true);
        onlineLoadButton.SetActive(true);
        FindObjectOfType<SavingWrapper>().Save();
    }

    object ISaveable.CaptureState()
    {
        return isRegistered;
    }

    void ISaveable.RestoreState(object state)
    {
        isRegistered =(bool) state;
    }
}
