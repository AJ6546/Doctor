using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] InventorySlot inventorySlotPrefab = null;
    PlayerInventory playerInventory;
    void Awake()
    {
        playerInventory = PlayerInventory.GetPlayerInventory();
        playerInventory.inventoryUpdated += Redraw;
    }

    void Start()
    {
        Redraw();
    }

    // Called whenever the inventory is updated.
    // Redraws the whole inventory to include changes
    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerInventory.GetSize(); i++)
        {
            var itemUI = Instantiate(inventorySlotPrefab, transform);
            itemUI.Setup(playerInventory, i);
        }
    }
}
