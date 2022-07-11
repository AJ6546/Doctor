using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A, B, C, D, E
    }
    [SerializeField] int buildIndex = -1;
    [SerializeField] Button sceneLoader;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;
    [SerializeField] int sceneToLoad;

    void Awake()
    {
       buildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void Start()
    {
        if(sceneLoader!=null)
            sceneLoader.gameObject.SetActive(false);
    }
    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(buildIndex + 1));
    }
    public void LoadPreviousScene()
    {
        StartCoroutine(LoadScene(buildIndex - 1));
    }
    public void LoadCurrentScene()
    {
        StartCoroutine(LoadScene(buildIndex));
    }
    private IEnumerator LoadScene(int sceneToLoad)
    {
        if (sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.enabled = false;

        yield return fader.FadeOut(fadeOutTime);

        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        newPlayerController.enabled = false;


        savingWrapper.Load();

        // Portal otherPortal = GetOtherPortal();
        // UpdatePlayer(otherPortal);

        savingWrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);
        fader.FadeIn(fadeInTime);

        newPlayerController.enabled = true;
        Destroy(gameObject);
    }
    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }

        return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneLoader.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        sceneLoader.gameObject.SetActive(false);
    }
}
