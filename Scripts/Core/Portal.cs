using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    // Used when player switches scene
    [SerializeField] int buildIndex = -1;
    [SerializeField] Button sceneLoader; // needs to be clicked to change scene
    [SerializeField] KeyCode interactionKeyboardButton = KeyCode.F;
    // Fading Times
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    [SerializeField] int sceneToLoad;
    [SerializeField] bool loadScene=false;
    void Awake()
    {
       buildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void Start()
    {
        if(sceneLoader!=null)
            sceneLoader.gameObject.SetActive(false);
    }
    private void Update()
    {
        // If Player is in middle of conversation, Do not do below
        if (FindObjectOfType<PlayerConversant>().IsTalking()) return;
        // If Player is Saving the game online Do not do below
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;

        if (Input.GetKeyDown(interactionKeyboardButton) && loadScene)
        {
            if (sceneToLoad != buildIndex)
                StartCoroutine(LoadScene(sceneToLoad));
            else
                LoadCurrentScene();
        }
    }
    // Load a specific scene by its build index
    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(LoadScene(index));
    }
    // Reload the same scene, usually for when palayer dies
    public void LoadCurrentScene()
    {
        StartCoroutine(LoadScene(buildIndex));
    }

    // Coroutine to load a scene
    private IEnumerator LoadScene(int sceneToLoad)
    {
        if (sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }
       
        // Do not destroy the portal before below is executed
        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.enabled = false;
        //fading out
        yield return fader.FadeOut(fadeOutTime);
        // saving
        savingWrapper.Save();
        // load the scene
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        newPlayerController.enabled = false;

        // loading the saved data from new scene
        savingWrapper.Load();
        // Saving new data from new scene
        savingWrapper.Save();
        // Fading in
        yield return new WaitForSeconds(fadeWaitTime);
        fader.FadeIn(fadeInTime);

        newPlayerController.enabled = true;
        // Ready to destry the portal
        Destroy(gameObject);
        // Saving
        savingWrapper.Save();
    }

    // Enable sceneLoader button when player is near portal.
    private void OnTriggerEnter(Collider other)
    {
        if (sceneLoader!=null && other.CompareTag("Player"))
        {
            loadScene = true;
            sceneLoader.gameObject.SetActive(true);
        }
    }

    // Disable sceneLoader button when player is far from portal.
    private void OnTriggerExit(Collider other)
    {
        loadScene = false;
        if (sceneLoader != null)
            sceneLoader.gameObject.SetActive(false);
    }
}
