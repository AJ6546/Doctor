using Cinemachine;
using UnityEngine;

public class OnLoad : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas gameUi;
    [SerializeField] Canvas introUi;
    void Start()
    {
        camera = Camera.main;
        gameUi.enabled = false;
    }

    
    public void OnGameStart()
    {
        camera.GetComponent<CinemachineBrain>().enabled = false;
        gameUi.enabled = true;
        introUi.enabled = false;
    }

}
