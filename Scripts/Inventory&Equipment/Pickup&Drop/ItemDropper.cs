using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour, ISaveable
{
    private List<Pickup> droppedItems = new List<Pickup>();

    public void DropItem(Item item, int number)
    {
        SpawnPickup(item, GetDropLocation(), number);
    }
    public void DropItem(Item item)
    {
        SpawnPickup(item, GetDropLocation(), 1);
    }
    public void SpawnPickup(Item item, Vector3 spawnLocation, int number)
    {
        var pickup = item.SpawnPickup(spawnLocation, number);
    }

    Vector3 GetDropLocation()
    {
        return new Vector3(transform.position.x,
            transform.position.y+0.5f, transform.position.z);
    }


    [System.Serializable]
    private struct DropRecord
    {
        public string itemID;
        public SerializableVector3 position;
        public int number;
    }

    object ISaveable.CaptureState()
    {
        RemoveDestroyedDrops();
        var droppedItemsList = new DropRecord[droppedItems.Count];
        for (int i = 0; i < droppedItemsList.Length; i++)
        {
            droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
            droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
            droppedItemsList[i].number = droppedItems[i].GetNumber();
        }
        return droppedItemsList;
    }

    void ISaveable.RestoreState(object state)
    {
        var droppedItemsList = (DropRecord[])state;
        foreach (var item in droppedItemsList)
        {
            var pickupItem = Item.GetFromID(item.itemID);
            Vector3 position = item.position.ToVector();
            int number = item.number;
            SpawnPickup(pickupItem, position, number);
        }
    }

    private void RemoveDestroyedDrops()
    {
        var newList = new List<Pickup>();
        foreach (var item in droppedItems)
        {
            if (item != null)
            {
                newList.Add(item);
            }
        }
        droppedItems = newList;
    }
}
