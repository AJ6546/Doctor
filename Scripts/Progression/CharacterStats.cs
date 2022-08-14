using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Range(1, 60)]
    [SerializeField] int startingLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;
    [SerializeField] int currentLevel = 1;
    [SerializeField] string levelUpParticleEffect = null,levelUpSoundEffect;
    AudioManager audioManager;
    public event Action onLevelUp;
    PoolManager poolManager;
    private void Start()
    {
        poolManager = PoolManager.instance;
        audioManager = AudioManager.instance;
        currentLevel = CalculateLevel();
        Experience experience = GetComponent<Experience>();
        if (experience != null)
        {
            experience.onExperienceGained += UpdateLevel;
        }
    }
    private void UpdateLevel()
    {
        int newLevel = CalculateLevel();
        if (newLevel > currentLevel)
        {
            currentLevel = newLevel;
            if (CompareTag("Player"))
                LevelUpEffect();
            onLevelUp();
        }
    }

    private void LevelUpEffect()
    {
        poolManager.Spawn(levelUpParticleEffect, transform.position, transform);
        audioManager.Play(levelUpSoundEffect, transform.position);
    }

    public float GetStat(Stats stat)
    {
        return progression.GetStat(stat, characterClass, CalculateLevel());
    }
    public int GetLevel()
    {
        if (currentLevel < 1)
        {
            currentLevel = CalculateLevel();
        }
        return currentLevel;
    }
    public int CalculateLevel()
    {
        Experience exp = GetComponent<Experience>();
        if (exp == null)
            return startingLevel;
        float currentXP = exp.GetExperience();
        int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
        for (int level = currentLevel; level < penultimateLevel; level++)
        {
            float XPToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
            if (XPToLevelUp > currentXP)
            {
                return level;
            }
        } 
        return penultimateLevel + 1;
    }
    
    public float CurrentLevel()
    {
        return currentLevel;
    }
}
