using Cinemachine;
using UnityEngine;
// Used to play a cenmatic intro when the player starts the game for the first time
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

    // when start game button is pressed
    public void OnGameStart()
    {
        camera.GetComponent<CinemachineBrain>().enabled = false;
        gameUi.enabled = true;
        introUi.enabled = false;
        FindObjectOfType<IntroPlayer>().SetPlayed(true);
    }
    // When replay button is pressed
    public void OnReplay()
    {
        GameObject.FindObjectOfType<Portal>().LoadCurrentScene();
    }
}
