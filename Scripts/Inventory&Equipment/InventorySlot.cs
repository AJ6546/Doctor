using UnityEngine;

// Script attached to InventorySlot
public class InventorySlot : MonoBehaviour, IDragContainer<Item>
{
    [SerializeField] InventoryItem icon = null;
    
    int index;
    Item item;
    PlayerInventory inventory;

    public void Setup(PlayerInventory inventory, int index)
    {
        this.inventory = inventory;
        this.index = index;
        item = GetItem();
        icon.SetItem(inventory.GetItemFromSlot(index), inventory.GetNumberInSlot(index));
    }

    // Returns item in this slot
    public Item GetItem()
    {
        return inventory.GetItemFromSlot(index);
    }
    // Returns number of item in this slot
    public int GetNumber()
    {
        return inventory.GetNumberInSlot(index);
    }
    // Removes Item from this slot
    public void RemoveItem(int number)
    {
        inventory.RemoveFromSlot(index, number);
    }
    // Adds an item to this slot
    public void AddItems(Item item, int number)
    {
        inventory.AddItemToSlot(index, item, number);
    }
    // Returns the maximum capacity of this slot
    public int MaxAcceptable(Item item)
    {
        if (inventory.HasSpaceForItem(item))
        { return int.MaxValue; }
        return 0;
    }
    // Uses an item and destroys it after use if it is consumable
    public void UseItem(int number)
    {
        if (item != null)
        {
            item.Use();
            if(item.isConsumable) RemoveItem(number);
        }
    }
}
