using UnityEngine;

// used to create an array of all fixed buttons and set them in a specific order,
// so they can be used fro other scripts by getting the index of the array element
public class UIAssigner : MonoBehaviour
{
    [SerializeField] FixedButton[] fixedButtons;
    [SerializeField] FixedButton[] fixedButtonsList = new FixedButton[9];
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
            if (f.name == "UnequipButton")
            {
                fixedButtonsList[2] = f;
            }
            if (f.name == "SaveButton")
            {
                fixedButtonsList[3] = f;
            }
            if (f.name == "LoadButton")
            {
                fixedButtonsList[4] = f;
            }
            if( f.name == "ControlsButton")
            {
                fixedButtonsList[5] = f;
            }
            if (f.name == "Attack01")
            {
                fixedButtonsList[6] = f;
            }
            if (f.name == "Attack02")
            {
                fixedButtonsList[7] = f;
            }
            if (f.name == "Attack03")
            {
                fixedButtonsList[8] = f;
            }
            if (f.name == "Attack04")
            {
                fixedButtonsList[9] = f;
            }
            
        }
    }

    // Returs an array of all the fixed buttons
    public FixedButton[] GetFixedButtons()
    {
        return fixedButtonsList;
    }
}
