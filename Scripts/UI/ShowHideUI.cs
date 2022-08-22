using UnityEngine;

// hides a pop up on start and on clicking close button
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
