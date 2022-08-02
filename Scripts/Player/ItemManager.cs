using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour,ISaveable
{

    public Equipment[] currentEquipment;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] Transform leftHandTransform;
    [SerializeField] Transform rightHandTransform;
    string handItem = "HandItem";
    [SerializeField] Fighter fighter;
    [SerializeField] FixedButton unEquipButton;
    [SerializeField] Equipment defaultEquipment, zeroEquipment;

    [SerializeField] TextMeshProUGUI title, description;
    private void Awake()
    {
        fighter = GetComponent<Fighter>();
        inventory = GetComponent<PlayerInventory>();
    }
    void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        if(defaultEquipment!=null)
            Equip(defaultEquipment);
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot;
        Equipment oldItem = null;
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            if (oldItem.GetDisplayName() != "Default")
            { inventory.AddToFirstEmptySlot(oldItem, 1); }
        }
        defaultEquipment = newItem;
        currentEquipment[slotIndex] = newItem;
        EquipItem(newItem);

    }

    public void Use(string displayName, string description, float modifier, bool isPercent, ItemType itemType, float lastingTime)
    {
        switch ((int)itemType)
        {
            case 0:
                fighter.SetDamageModifier(modifier);
                break;
            case 1:
                DisplayDetails(displayName, description);
                break;
        }
    }

    private void DisplayDetails(string displayName, string description)
    {
        title.text = displayName;
        this.description.text = description;
    }

    void EquipItem(Equipment equipment)
    {
        switch (equipment.equipmentSlot)
        {
            case EquipmentSlot.hands:
                DestroyOldWeapon(rightHandTransform, leftHandTransform);
                if (equipment.equipedPrefab1 != null && equipment.equipedPrefab2 != null)
                {
                    GameObject leftHand = Instantiate(equipment.equipedPrefab1, leftHandTransform);
                    GameObject rightHand = Instantiate(equipment.equipedPrefab2, rightHandTransform);
                    leftHand.name = handItem;
                    rightHand.name = handItem;
                }
                else
                {
                    Transform handTransform = equipment.isRightHanded ? rightHandTransform : leftHandTransform;
                    GameObject hand = Instantiate(equipment.equipedPrefab1, handTransform);
                    hand.name = handItem;
                }
                break;
        }
        if (currentEquipment[2].overrideController != null && currentEquipment[2] != null)
        {
            fighter.playerAnimator.runtimeAnimatorController = currentEquipment[2].overrideController;
            fighter.SetDamageModifier(currentEquipment[2].damageModifier);
        }
    }
    void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon1 = rightHand.Find(handItem);
        Transform oldWeapon2 = leftHand.Find(handItem);
        if (oldWeapon1 == null && oldWeapon2 == null) return;
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
        if (Input.GetKeyDown("u") || unEquipButton.Pressed)
        {
            UnEquip(2);
        }
    }

    public object CaptureState()
    {
        return defaultEquipment.itemID;
    }

    public void RestoreState(object state)
    {
        defaultEquipment = (Equipment)(Item.GetFromID((string)state));
    }

    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equip(zeroEquipment);
        }
    }
}
