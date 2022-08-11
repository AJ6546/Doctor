using System.Collections.Generic;
using UnityEngine;

public class ClueCollector : MonoBehaviour, ISaveable
{
    [SerializeField] int cluesCollected=0;
    public List<string> clueItems = new List<string>();
    public List<string> collectedClues = new List<string>();
    [SerializeField] Object[] itemList;
    [SerializeField] string disease;
    private void Start()
    {
        disease = GetComponent<Disease>().GetDisease();
        itemList = Resources.LoadAll<Item>(disease);
    }
    private void Update()
    {
        if(clueItems.Count<=0)
        {
            clueItems = GetClues();
        }
    }
    public List<string> GetClues()
    {
        List<string> clues = new List<string>();
        foreach (Object item in itemList)
        {
            Item itm = (Item)item;
            clues.Add(itm.itemID);
        }
        return clues;
    }
    public void UpdateCluesCollected(string itemID)
    {
        if(clueItems.Contains(itemID) && !collectedClues.Contains(itemID))
        {
            collectedClues.Add(itemID);
            cluesCollected++;
        }
    }
    public bool AllCluesCollected()
    {
        return clueItems.Count <= collectedClues.Count;
    }
    object ISaveable.CaptureState()
    {
        string[] clues = new string[collectedClues.Count];
        for (int i = 0; i < collectedClues.Count; i++)
        {
            clues[i] = collectedClues[i];
        }
        return clues;
    }

    void ISaveable.RestoreState(object state)
    {
        string[] clues = (string[])state;
        for (int i = 0; i < clues.Length; i++)
        {
            if(!collectedClues.Contains(clues[i]))
                collectedClues.Add(clues[i]);
        }
    }

    public void RemoveCollectedClues()
    {
        cluesCollected = 0;
        collectedClues.Clear();
    }
}
