using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour
{
    [SerializeField] GameObject textContainer = null;
    [SerializeField] TextMeshProUGUI itemNumber = null;

    // Calls SetItem with number=0 (Item is not stackable)
    public void SetItem(Item item)
    {
        SetItem(item, 0);
    }

    // Sets Item in inventory by enabling its Image component and setting sprite to icon of item 
    // present in the slot. If the number of items a slot holds is > 0 enables the text field and 
    // sets the number
    public void SetItem(Item item, int number)
    {
        var icon = GetComponent<Image>();
        if (item == null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = item.GetIcon();
        }
        if (itemNumber)
        {
            if (number <= 1)
            {
                textContainer.SetActive(false);
            }
            else
            {
                textContainer.SetActive(true);
                itemNumber.text = number.ToString();
            }
        }
    }

}
