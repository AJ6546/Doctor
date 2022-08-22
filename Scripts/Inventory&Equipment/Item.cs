using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object for Item
[CreateAssetMenu(menuName = ("Inventory/Item"))]
public class Item : ScriptableObject, ISerializationCallbackReceiver
{
    public string itemID = null; // Unique ID of item
    [SerializeField] string displayName = null; // Display name of Item
    [SerializeField] [TextArea] string description = null; // Short Description of Item
    [SerializeField] Sprite icon = null; // Icon the item should show in inventory
    [SerializeField] bool stackable = false; // is the item stackable ?
    [SerializeField] Pickup pickup = null; 
    static Dictionary<string, Item> itemLookupCache; // Dictionary holding all item id's mapped to item
    [SerializeField] float modifier; // modifier amount
    public bool  isConsumable; // Is Itemdesto=royed after use ?
    [SerializeField] ItemType itemType;

    public static Item GetFromID(string itemID)
    {
        // populates itemLookupCache dictionary
        if (itemLookupCache == null)
        {
            itemLookupCache = new Dictionary<string, Item>();
            // Loading all items from dictionary
            var itemList = Resources.LoadAll<Item>("");

            // Looping through itemList and adding to dictionary if not already present
            foreach (var item in itemList)
            {
                if (itemLookupCache.ContainsKey(item.itemID))
                    continue;
                itemLookupCache[item.itemID] = item;
            }
        }
        if (itemID == null || !itemLookupCache.ContainsKey(itemID)) {  return null; }

        // returns item for a given item ID
        return itemLookupCache[itemID];
    }

    // Getters
    public string GetItemID()
    {
        return itemID;
    }
    public string GetDisplayName()
    {
        return displayName;
    }
    public string GetDescription()
    {
        return description;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public bool IsStackable()
    {
        return stackable;
    }

    // Used to spawn pickups at a specified position
    public Pickup SpawnPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.pickup);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    // Generating Uinique ID's
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }

    // Implementing this method to avoid errors
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    { }

    // Calls Use in Item Manager where it is decided what to do
    // If item is a weapon it is equiped
    // If item is a clue the description is displayed
    public virtual void Use()
    {
        FindObjectOfType<ItemManager>().Use(displayName, description, modifier, itemType);
    }
}

// Enum for types of items
public enum ItemType
{
    Weapon, Clue, None
}