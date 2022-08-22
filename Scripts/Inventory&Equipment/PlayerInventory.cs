using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ISaveable
{
   
    [SerializeField] int inventorySize = 10; // Number of slots in inventory

    InventorySlot[] slots;


    public struct InventorySlot
    {
        public Item item;
        public int number;
    }


    public event Action inventoryUpdated;

    public static PlayerInventory GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<PlayerInventory>();
    }

    // Returns true if there is a free slot where item can go
    public bool HasSpaceForItem(Item item)
    {
        return FindSlot(item) >= 0;
    }

    // returns number of slots
    public int GetSize()
    {
        return slots.Length;
    }

    public bool AddToFirstEmptySlot(Item item, int number)
    {
        int i = FindSlot(item); // Finding a slot
        if (i < 0)
        {
            return false;
        }
        // Updating Player's clue collector 
        GetComponent<ClueCollector>().UpdateCluesCollected(item.itemID);
        slots[i].item = item;

        // If the Item is stackable update number by number of items that is being added
        if (item.IsStackable())
        {
            slots[i].number += number;
        }
        // Update number by 1
        else { slots[i].number += 1; }

        // Update inventory UI
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        if (number > 1 && !item.IsStackable())
        {
            number--;
            AddToFirstEmptySlot(item, number);
        }
        return true;
    }

    // Returns true if player already has this item
    public bool HasItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    // Returns item in a specified slot
    public Item GetItemFromSlot(int slot)
    {
        return slots[slot].item;
    }

    // Returns number of items in a specified slot
    internal int GetNumberInSlot(int slot)
    {
        return slots[slot].number;
    }

    // Removes Item from a specified slot
    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].number -= number;
        if (slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }

        // Updating Inventory UI
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    // Removes all items in inventory
    public void RemoveAllItems()
    {
        for(int i=0;i<slots.Length;i++)
        {
            slots[i].number = 0;
            slots[i].item = null;
        }
    }

    // Add Item to a specified Slot
    public bool AddItemToSlot(int slot, Item item, int number)
    {
        // Add Item to first empty slot
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number);
        }
        // Add item to a slot where an ite of this type already exists and if item is stackable
        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }
        slots[slot].item = item;
        slots[slot].number += number;

        // Update Inventory UI
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }
    

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }

    int FindSlot(Item item)
    {
        // Add item to a slot where an ite of this type already exists and if item is stackable
        int i = FindStack(item);
        // Add Item to a new slot
        if (i < 0)
        {
            return FindEmptySlot(); 
        }
        return i;
    }

    // Find an empty slot to add the item
    int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }
    
    // Finding a slot where an item of this type already exists. Used to stack Item
    public int FindStack(Item item)
    {
        if (!item.IsStackable())
        {
            return -1; // If item is not stackable
        }

        // Returns index if item is stackable and there is aready an item of this type in inventory
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i; 
            }
        }
        return -1;
    }

    // Saving 
    [System.Serializable]
    private struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }

    object ISaveable.CaptureState()
    {
        var slotStrings = new InventorySlotRecord[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (slots[i].item != null)
            {
                slotStrings[i].itemID = slots[i].item.GetItemID();
                slotStrings[i].number = slots[i].number;
            }
        }
        return slotStrings;
    }

    void ISaveable.RestoreState(object state)
    {
        var slotStrings = (InventorySlotRecord[])state;
        for (int i = 0; i < inventorySize; i++)
        {
            slots[i].item = Item.GetFromID(slotStrings[i].itemID);
            slots[i].number = slotStrings[i].number;
        }

        // Updating Inventory UI
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }
}
