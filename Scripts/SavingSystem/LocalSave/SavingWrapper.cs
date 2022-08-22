using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingWrapper : MonoBehaviour
{
    [SerializeField] KeyCode saveKey = KeyCode.K;
    [SerializeField] KeyCode loadKey = KeyCode.L;
    [SerializeField] KeyCode deleteKey = KeyCode.Delete;
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField] FixedButton save, load;
    const string defaultSaveFile = "save";

    private void Awake()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        // If Player is talking, Do not do below
        if (FindObjectOfType<PlayerConversant>().IsTalking()) return;
        // If Player is Saving the game online Do not do below
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;
        if (uiAssigner == null || save == null || load == null)
        {
            uiAssigner = GameObject.FindGameObjectWithTag("Player").GetComponent<UIAssigner>();
            save = uiAssigner.GetFixedButtons()[3];
            load = uiAssigner.GetFixedButtons()[4];
        }
        if (Input.GetKeyDown(saveKey) || save.Pressed)
        {
            Save(); // Save Data on button press
        }
        if (Input.GetKeyDown(loadKey)|| load.Pressed)
        {
            Load(); // Load Data on button press
        }
        if (Input.GetKeyDown(deleteKey))
        {
            Delete(); // Delete Saved Data on button press
        }
    }
    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
