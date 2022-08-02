using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] int buildIndex = -1;
    [SerializeField] Button sceneLoader;
    [SerializeField] KeyCode interactionKeyboardButton = KeyCode.F;
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
        if(Input.GetKeyDown(interactionKeyboardButton) && loadScene)
        {
            if (sceneToLoad > buildIndex)
                LoadNextScene();
            else if (sceneToLoad < buildIndex)
                LoadPreviousScene();
            else
                LoadCurrentScene();
        }
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

        savingWrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);
        fader.FadeIn(fadeInTime);

        newPlayerController.enabled = true;
        Destroy(gameObject);
        savingWrapper.Save();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (sceneLoader!=null && other.CompareTag("Player"))
        {
            loadScene = true;
            sceneLoader.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        loadScene = false;
        if (sceneLoader != null)
            sceneLoader.gameObject.SetActive(false);
    }
}
