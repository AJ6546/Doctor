using System.Collections.Generic;
using UnityEngine;

public class ClueCollector : MonoBehaviour, ISaveable
{
    [SerializeField] int cluesCollected=0; // numberof clues collected
    public List<string> clueItems = new List<string>(); // list of all clues for the disease
    public List<string> collectedClues = new List<string>(); // list of collected clues
    [SerializeField] Object[] itemList;
    [SerializeField] string disease; // disease patient has
    private void Start()
    {
        disease = GetComponent<Disease>().GetDisease(); // disease the patient has
        itemList = Resources.LoadAll<Item>(disease); // load all clues for the disease
    }
    private void Update()
    {
        if(clueItems.Count<=0)
        {
            clueItems = GetClues(); // getting clues
        }
    }

    // converts clues from object array to a list of strings and returns it.
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

    // Updates a list with ll collected clues
    public void UpdateCluesCollected(string itemID)
    {
        if(clueItems.Contains(itemID) && !collectedClues.Contains(itemID))
        {
            collectedClues.Add(itemID);
            cluesCollected++;
        }
    }

    // becomes true when plaer has collected all the clues
    public bool AllCluesCollected()
    {
        return clueItems.Count <= collectedClues.Count;
    }

    // Remove all collected clues
    public void RemoveCollectedClues()
    {
        cluesCollected = 0;
        collectedClues.Clear();
    }

    // Saving collected clues as a strig array
    object ISaveable.CaptureState()
    {
        string[] clues = new string[collectedClues.Count];
        for (int i = 0; i < collectedClues.Count; i++)
        {
            clues[i] = collectedClues[i];
        }
        return clues;
    }

    // Loading from state to a string array and updating list of collected clues
    void ISaveable.RestoreState(object state)
    {
        string[] clues = (string[])state;
        for (int i = 0; i < clues.Length; i++)
        {
            if(!collectedClues.Contains(clues[i]))
                collectedClues.Add(clues[i]);
        }
    }
}
