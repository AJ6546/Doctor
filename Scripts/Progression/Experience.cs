using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour, ISaveable
{
    [SerializeField] float experiencePoints = 0;
    [SerializeField] float maxExperience = 100;
    public event Action onExperienceGained;
    [SerializeField] int maxLevel;
    [SerializeField] int minLevel=0;
    private void Start()
    {
        maxExperience = GetComponent<CharacterStats>().GetStat(Stats.ExperienceToLevelUp);
    }
    public void GainExperience(float experience)
    {
        if (maxLevel <= GetComponent<CharacterStats>().CalculateLevel()) return;
        if(minLevel>= GetComponent<CharacterStats>().CalculateLevel()) return;
        if (experiencePoints + experience < 0) { experiencePoints = 0; return; }
        
        experiencePoints += experience;
        onExperienceGained();
        maxExperience = GetComponent<CharacterStats>().GetStat(Stats.ExperienceToLevelUp);
    }
    public float GetExperienceFraction()
    {
        return experiencePoints / maxExperience;
    }
    public float GetExperience()
    {
        return experiencePoints;
    }
    public float GetMaxExperience()
    {
        return maxExperience;
    }
    public object CaptureState()
    {
        return experiencePoints;
    }

    public void RestoreState(object state)
    {
        experiencePoints = (float)state;
    }
}
