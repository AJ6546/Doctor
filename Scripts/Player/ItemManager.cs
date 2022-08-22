using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour,ISaveable
{

    public Equipment[] currentEquipment; // Array of current equipments the player has
    [SerializeField] PlayerInventory inventory;
    [SerializeField] Transform leftHandTransform; // trasform for where the weapon goes in left hand
    [SerializeField] Transform rightHandTransform; // transform for where the weapon goes in right hand
    string handItem = "HandItem"; // name of weapon
    [SerializeField] Fighter fighter;
    [SerializeField] FixedButton unEquipButton;
    // default and zero equipments are set to empty equipment at beginning of the game.
    // default equipment is used to save current weapon player is holding
    // zero equipment is used to set empty equipment when all weapons are removed
    [SerializeField] Equipment defaultEquipment, zeroEquipment;

    [SerializeField] TextMeshProUGUI title, description;
    private void Awake()
    {
        fighter = GetComponent<Fighter>(); // ref to Fighter
        inventory = GetComponent<PlayerInventory>(); // ref to inventory
    }
    void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; // number of slots where item can be equiped
        currentEquipment = new Equipment[numSlots]; 
        if(defaultEquipment!=null)
            Equip(defaultEquipment); // equiping default equipment
    }

    // Called to equip a new item
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot; // setting slot index to equp the new item
        Equipment oldItem = null;
        // If there is already an equipment we swap the 2 equipments
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex]; // oldItem is set with current weapon player has
            if (oldItem.GetDisplayName() != "Default") 
            {
                // If the item is not default equipment send old item to inventry.
                // Weapon items are not stackable, so number = 1
                inventory.AddToFirstEmptySlot(oldItem, 1);
            }
        }
        // Settig default item as new item
        defaultEquipment = newItem;
        // Setting current item in specific slot as new item
        currentEquipment[slotIndex] = newItem;
        // Equip new item
        EquipItem(newItem);
    }

    // Using an item based on its type
    public void Use(string displayName, string description, float modifier, ItemType itemType)
    {
        switch ((int)itemType)
        {
            case 0:
                // Item is a weapon, so increase the damage player attacks cause
                fighter.SetDamageModifier(modifier);
                break;
            case 1:
                // Item is a clue. Display the information
                DisplayDetails(displayName, description);
                break;
        }
    }

    // Displays clue details
    private void DisplayDetails(string displayName, string description)
    {
        title.text = displayName;
        this.description.text = description;
    }

    // Equiping an item depending on equipment slot
    void EquipItem(Equipment equipment)
    {
        switch (equipment.equipmentSlot)
        {
            // Currently we have only equipable weapons
            // more cases will be added when armors are introduced
            case EquipmentSlot.hands:
                // Destroy the weapon prefab that player was previously holding, if any
                DestroyOldWeapon(rightHandTransform, leftHandTransform);
                // If the equip prefabs of Equipment are not null
                // This is used in case of 2 handed weapons, where both the prefabs can't be null
                // currently we have only one handed weapons, so not in use
                if (equipment.equipedPrefab1 != null && equipment.equipedPrefab2 != null)
                {
                    // Equiping the weapons
                    GameObject leftHand = Instantiate(equipment.equipedPrefab1, leftHandTransform);
                    GameObject rightHand = Instantiate(equipment.equipedPrefab2, rightHandTransform);
                    // Setting names of equiped prefabs
                    leftHand.name = handItem;
                    rightHand.name = handItem;
                }
                // Used for one handed weapons
                else
                {
                    // Find the hand the weapon goes to
                    Transform handTransform = equipment.isRightHanded ? rightHandTransform : leftHandTransform;
                    // Equiping the weapons
                    GameObject hand = Instantiate(equipment.equipedPrefab1, handTransform);
                    // Setting names of equiped prefabs
                    hand.name = handItem;
                }
                break;
        }
        if (currentEquipment[2].overrideController != null && currentEquipment[2] != null)
        {
            // Setting new animaotr controller 
            fighter.playerAnimator.runtimeAnimatorController = currentEquipment[2].overrideController;
            // Setting damage Modifier
            fighter.SetDamageModifier(currentEquipment[2].damageModifier);
        }
    }

    // Used to destroy old weapons
    void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        // setting old weapon in both hands
        Transform oldWeapon1 = rightHand.Find(handItem);
        Transform oldWeapon2 = leftHand.Find(handItem);
        // If there are no weapon in either hands, return
        if (oldWeapon1 == null && oldWeapon2 == null) return;

        // If there is a weapon in aither of the hands, Destroy it
        if (oldWeapon1 != null)
        {
            Destroy(oldWeapon1.gameObject);
        }
        if (oldWeapon2 != null)
        {
            Destroy(oldWeapon2.gameObject);
        }
    }

    private void Update()
    {
        if (unEquipButton == null)
        {
            unEquipButton = GetComponent<UIAssigner>().GetFixedButtons()[2];
        }

        // UnEquip a weapon if button is pressed
        if (Input.GetKeyDown("u") || unEquipButton.Pressed)
        {
            UnEquip(2);
        }
    }

    // Unequiping a weapon is replacing current weapon with zero equipment
    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equip(zeroEquipment);
        }
    }

    // Saving current weapon
    public object CaptureState()
    {
        return defaultEquipment.itemID;
    }
    // Loading current weapon
    public void RestoreState(object state)
    {
        defaultEquipment = (Equipment)(Item.GetFromID((string)state));
    }

   
}
