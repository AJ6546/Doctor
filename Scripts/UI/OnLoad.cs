using Cinemachine;
using UnityEngine;

public class OnLoad : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas gameUi;
    [SerializeField] Canvas introUi;
    [SerializeField] bool sequencePlayed;
    void Start()
    {
        camera = Camera.main;
        gameUi.enabled = false;
        sequencePlayed = FindObjectOfType<IntroPlayer>().IsIntroPlayed();
        if (sequencePlayed)
            OnGameStart();
    }

    
    public void OnGameStart()
    {
        camera.GetComponent<CinemachineBrain>().enabled = false;
        gameUi.enabled = true;
        introUi.enabled = false;
        FindObjectOfType<IntroPlayer>().SetPlayed(true);
    }
    public void OnReplay()
    {
        GameObject.FindObjectOfType<Portal>().LoadCurrentScene();
    }
}
