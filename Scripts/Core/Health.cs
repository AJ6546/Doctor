using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, ISaveable
{
    [SerializeField]
    float healthPoints, startHealth = 50, regeneratePercent = 70;
    [SerializeField] bool isDead = false; 
    [SerializeField] string  sfx="EnemyDeath";
    [SerializeField] UnityEvent<float> takeDamage;
    [SerializeField] AudioManager audioManager;
    void Start()
    {
        audioManager = AudioManager.instance;
        startHealth = GetComponent<CharacterStats>().GetStat(Stats.Health);
        if (healthPoints <= 0||healthPoints>startHealth)
            healthPoints = startHealth;
        GetComponent<CharacterStats>().onLevelUp += RegenerateHealth;
    }

    public string GetStartHealth()
    {
        return ((int)startHealth).ToString();
    }

    public string GetHealth()
    {
        return ((int)healthPoints).ToString();
    }

    private void RegenerateHealth()
    {
        startHealth=healthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health);
        float regenHealthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health) * regeneratePercent / 100;
        healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
    }

    public void TakeDamage(GameObject instigator, float damage)
    {
        if (isDead)
            return;
        healthPoints = (int)Mathf.Lerp(healthPoints, Mathf.Max(healthPoints - damage, 0), Time.deltaTime * 1000);
        takeDamage.Invoke(damage);
        if (CompareTag("Player"))
            sfx = "PlayerDamage";
        else
            sfx = "EnemyDamage";
        audioManager.Play(sfx, transform.position);
        if (healthPoints <= 0)
        {
            if (instigator != gameObject)
                AwardExperience(instigator, true);
            Die();
        }
    }

    public void AwardExperience(GameObject instigator,bool positive)
    {
        Experience experience = instigator.GetComponent<Experience>();
        if (experience != null)
        {
            if(positive)
                experience.GainExperience(GetComponent<CharacterStats>().GetStat(Stats.Experience));
            else
                experience.GainExperience(-GetComponent<CharacterStats>().GetStat(Stats.Experience)/2);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        if (CompareTag("Player"))
            sfx = "PlayerDeath";
        else
            sfx = "EnemyDeath";
        audioManager.Play(sfx, transform.position);
        
        healthPoints = -1f;
        GetComponent<Animator>().SetTrigger("Death");
        if (CompareTag("Player"))
        {
            RegenerateHealth();
            GetComponent<ItemManager>().UnEquip(2);
            GetComponent<PlayerInventory>().RemoveAllItems();
            GetComponent<ClueCollector>().RemoveCollectedClues();
            AwardExperience(gameObject, false);
        }
        else
        {
            GetComponent<ClueDropper>().DropClue();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    object ISaveable.CaptureState()
    {
        return healthPoints;
    }

    void ISaveable.RestoreState(object state)
    {
        healthPoints = (float)state;
        if(healthPoints<0)
        {
            Die();
        }
    }
    void Update()
    {
        if(CompareTag("Player")&&Input.GetKeyDown(KeyCode.X)&&!IsDead())
        {
            TakeDamage(gameObject,5);
        }
    }

    public float GetHealthFraction()
    {
        return healthPoints / startHealth;
    }

 
    public void UpdateHealth(float updatePercent)
    {
        float hp = healthPoints;
        sfx = "HealthUp";
        audioManager.Play(sfx, transform.position);
        healthPoints = Mathf.Min(startHealth, healthPoints + (startHealth * updatePercent / 100));
    }
}
