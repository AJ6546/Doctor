using System.Collections.Generic;
using UnityEngine;

public class ClueDropper : MonoBehaviour
{
    [SerializeField] Object[] itemList;
    [SerializeField] string disease;
    [SerializeField] int num;
    [SerializeField] List<string> uncollectedClues = new List<string>();
    ClueCollector clueCollector;
    void Start()
    {
        disease = FindObjectOfType<Disease>().GetDisease();
        itemList = Resources.LoadAll<Item>(disease);
        clueCollector = FindObjectOfType<ClueCollector>();
        UpdateUncollectedClues();
    }

    public void DropClue()
    {
        num = Random.Range(0, 10);
        if (num > 7)
        {
            int index = Random.Range(0, itemList.Length);
            GetComponent<ItemDropper>().DropItem((Item)itemList[index]);
        }
        else
        {
            UpdateUncollectedClues();
            int index = Random.Range(0, uncollectedClues.Count);
            GetComponent<ItemDropper>().DropItem(Item.GetFromID(uncollectedClues[index]));
        }
    }
    void UpdateUncollectedClues()
    {
        uncollectedClues.Clear();
        foreach (string clue in clueCollector.clueItems)
        {
            if (!clueCollector.collectedClues.Contains(clue))
            {
                uncollectedClues.Add(clue);
            }
        }
    }
}
