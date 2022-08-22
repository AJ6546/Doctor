using UnityEngine;

// A scriptable object inheriting from Item
[CreateAssetMenu(menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot; // slot where the item can be equiped

    // position and rotation of equiped item/s
    public GameObject equipedPrefab1 = null; 
    public GameObject equipedPrefab2 = null;

    // If the item goes in rightHand or not
    public bool isRightHanded;

    // Used to change animation when player is holding an item (weapon)
    public AnimatorOverrideController overrideController = null;

    // Increase in damage player causes while holding the item
    public float damageModifier = 0;

    public override void Use()
    {
        FindObjectOfType<ItemManager>().Equip(this);
    }
}

// Enumeratior for different slots where an item goes
public enum EquipmentSlot
{
    head, legs, hands, chest, feet
}