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
        // if adding the experience would let the character level up above maxLevel 
        // or minLevel, we do not increment experience.
        if (maxLevel <= GetComponent<CharacterStats>().CalculateLevel()) return;
        if(minLevel>= GetComponent<CharacterStats>().CalculateLevel()) return;
        if (experiencePoints + experience < 0) { experiencePoints = 0; return; }
        
        experiencePoints += experience;
        onExperienceGained(); 
        maxExperience = GetComponent<CharacterStats>().GetStat(Stats.ExperienceToLevelUp);
    }

    // Getters and Setters for experience and maxExperience
    public float GetExperience()
    {
        return experiencePoints;
    }
  
    public float GetMaxExperience()
    {
        return maxExperience;
    }

    public void SetExperience(float experiencePoints)
    {
        this.experiencePoints= experiencePoints;
    }

    public void  SetMaxExperience(float maxExperience)
    {
        this.maxExperience=maxExperience;
    }

    // Saving
    public object CaptureState()
    {
        return experiencePoints;
    }
    // Loading
    public void RestoreState(object state)
    {
        experiencePoints = (float)state;
    }
}
