using UnityEngine;

public class ShowHideUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
