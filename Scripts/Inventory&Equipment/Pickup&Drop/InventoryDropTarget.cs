using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDropTarget : MonoBehaviour, IDragDestination<Item>
{
    public void AddItems(Item item, int number)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemDropper>().DropItem(item, number);
    }
    public int MaxAcceptable(Item item)
    {
        return int.MaxValue;
    }
}
