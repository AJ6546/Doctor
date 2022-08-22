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
        poolManager = PoolManager.instance; // instance of pool manager
        audioManager = AudioManager.instance; // instance of audio manager
        currentLevel = CalculateLevel(); // getting current level
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
            // play level up effects for player
            if (CompareTag("Player"))
                LevelUpEffect();
            onLevelUp();
        }
    }

    // Play sfx and vfx on level up
    private void LevelUpEffect()
    {
        poolManager.Spawn(levelUpParticleEffect, transform.position, transform);
        audioManager.Play(levelUpSoundEffect, transform.position);
    }
    
    // returns stats for current level
    public float GetStat(Stats stat)
    {
        return progression.GetStat(stat, characterClass, CalculateLevel());
    }
    public int GetLevel()
    {
        if (currentLevel < 1)
        {
            // current level is set up
            currentLevel = CalculateLevel();
        }
        return currentLevel;
    }

    // After Gaining EExperience 
    public int CalculateLevel()
    {
        Experience exp = GetComponent<Experience>();
        if (exp == null)
            return startingLevel;
        float currentXP = exp.GetExperience();

        // second last level
        int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
        for (int level = currentLevel; level < penultimateLevel; level++)
        {
            float XPToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
            if (XPToLevelUp > currentXP)
            {
                // If XPToLevelUp is greater than currentXP, character remains the same level and loop stops here.
                return level;
            }
        } 
        // if the character has experience more than penultmate level always return the final level.
        return penultimateLevel + 1;
    }
    
    public float CurrentLevel()
    {
        return currentLevel;
    }
}
