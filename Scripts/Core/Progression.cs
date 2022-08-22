using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
public class Progression : ScriptableObject
{

    [SerializeField] ProgressionCharacterClass[] characterClasses;

    Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;
    public float GetStat(Stats stat, CharacterClass characterClass, int level)
    {
        BuildLookup();
        float[] levels = lookupTable[characterClass][stat];
        if (levels.Length < level) return 0;
        else return levels[level - 1];
    }
    public int GetLevels(Stats stat, CharacterClass characterClass)
    {
        BuildLookup();
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }
    
    // Creating a Dictionary LookupTable
    // Loops over all the ProgressionCharacterClass elements.
    // Inside this we loop through all the ProgressionStat in each ProgressionCharacterClass 
    // and add them to a dictionary called statLookupTable, with the stat type as key and level as value.
    // Then for each ProgressionCharacterClass we have another dictionary lookupTable which takes character type 
    // as key and its statLookupTable as value.
    private void BuildLookup()
    {
        if (lookupTable != null) return;
        lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();
        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            var statLookupTable = new Dictionary<Stats, float[]>();
            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levels;
            }
            lookupTable[progressionClass.characterClass] = statLookupTable;
        }
    }

    // Represents each Character Calls and their stats that progresses over time.
    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;

        public ProgressionStat[] stats;
    }

    // Represents each stats and their value depending on the level
    [System.Serializable]
    class ProgressionStat
    {
        public Stats stat;
        public float[] levels;
    }
}
