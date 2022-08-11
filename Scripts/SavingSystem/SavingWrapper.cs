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
        if (FindObjectOfType<PlayerConversant>().IsTalking()) return;
        if (uiAssigner == null || save == null || load == null)
        {
            uiAssigner = GameObject.FindGameObjectWithTag("Player").GetComponent<UIAssigner>();
            save = uiAssigner.GetFixedButtons()[3];
            load = uiAssigner.GetFixedButtons()[4];
        }
        if (Input.GetKeyDown(saveKey) || save.Pressed)
        {
            Save();
        }
        if (Input.GetKeyDown(loadKey)|| load.Pressed)
        {
            Load();
        }
        if (Input.GetKeyDown(deleteKey))
        {
            Delete();
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
