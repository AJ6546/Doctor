using UnityEngine;

public class UIAssigner : MonoBehaviour
{
    [SerializeField] FixedButton[] fixedButtons;
    [SerializeField] FixedButton[] fixedButtonsList = new FixedButton[3];
    private void Awake()
    {
        fixedButtons = FindObjectsOfType<FixedButton>();     

        foreach (FixedButton f in fixedButtons)
        {
            if (f.name == "JumpButton")
            {
                fixedButtonsList[0] = f;
            }
            if (f.name == "InventoryButton")
            {
                fixedButtonsList[1] = f;
            }
            if (f.name == "SceneLoader")
            {
                fixedButtonsList[2] = f;
            }
        }
    }
    public FixedButton[] GetFixedButtons()
    {
        return fixedButtonsList;
    }
}
