using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, ISaveable
{
    // This is the health of the character. Same script is used for Player, Enemy and NPCs
    [SerializeField]
    // StartHealth and current health. When a character dies the characters,
    // health regenerates up to 70% of start health. 
    float healthPoints, startHealth = 50, regeneratePercent = 70;
    [SerializeField] bool isDead = false; // A boolean that says if the character is alive
    [SerializeField] string  sfx="EnemyDeath"; // sfx to be played when a character dies.
    [SerializeField] UnityEvent<float> takeDamage; // This event is triggered when character takes damage
    [SerializeField] AudioManager audioManager; // Instance of Audio Manager
    void Start()
    {
        audioManager = AudioManager.instance;
        // setting the health according to the Progression lookup table.
        startHealth = GetComponent<CharacterStats>().GetStat(Stats.Health);
        // setting healthpoints
        if (healthPoints <= 0||healthPoints>startHealth)
            healthPoints = startHealth;
        // Regenerated health when player levels up
        GetComponent<CharacterStats>().onLevelUp += RegenerateHealth;
    }

   
    // Called when Player dies or levels up. 
    private void RegenerateHealth()
    {
        startHealth=healthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health);
        float regenHealthPoints = GetComponent<CharacterStats>().GetStat(Stats.Health) * regeneratePercent / 100;
        healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
    }

    // Called when an attack damages the character
    public void TakeDamage(GameObject instigator, float damage)
    {
        // A dead character does not take any more damage.
        if (isDead)
            return;
        healthPoints = (int)Mathf.Lerp(healthPoints, Mathf.Max(healthPoints - damage, 0), Time.deltaTime * 1000);
        takeDamage.Invoke(damage);

        // Play sfx on taking damage
        if (CompareTag("Player"))
            sfx = "PlayerDamage";
        else
            sfx = "EnemyDamage";
        audioManager.Play(sfx, transform.position);

        // If healthPoints <=0 the character dies and the one who attacked it gains experience
        if (healthPoints <= 0)
        {
            if (instigator != gameObject)
                AwardExperience(instigator, true);
            Die();
        }
    }

    // Method to gain or lose experience. 
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

        // Play SFX on death
        if (CompareTag("Player"))
            sfx = "PlayerDeath";
        else
            sfx = "EnemyDeath";
        audioManager.Play(sfx, transform.position);
        
        // Setting the healthpoint to negative. So that on start it gets reset.
        healthPoints = -1f;

        // Play Death animation
        GetComponent<Animator>().SetTrigger("Death");
        if (CompareTag("Player"))
        {
            RegenerateHealth();
            // Player looses equiped weapon
            GetComponent<ItemManager>().UnEquip(2);
            // Player looses all items in inventory
            GetComponent<PlayerInventory>().RemoveAllItems();
            // Player looses all clues he collected
            GetComponent<ClueCollector>().RemoveCollectedClues();
            // Player looses experience 
            AwardExperience(gameObject, false);
        }
        else
        {
            // Enemy drops clue item
            GetComponent<ClueDropper>().DropClue();
            // Updates kill count
            FindObjectOfType<Achiever>().UpdateKillCount();
        }
        // To not let the enemy body drop where the clue item drops
        transform.position = transform.position + transform.forward * -1f;
    }

    // Used to update health of player when he picks up a health item  
    public void UpdateHealth(float updatePercent)
    {
        float hp = healthPoints;
        sfx = "HealthUp";
        // Play SFX on health pickup
        audioManager.Play(sfx, transform.position);
        // Updaing health by some % of start health
        healthPoints = Mathf.Min(startHealth, healthPoints + (startHealth * updatePercent / 100));
    }

    // Getters
    public string GetStartHealth()
    {
        return ((int)startHealth).ToString();
    }

    public string GetHealth()
    {
        return ((int)healthPoints).ToString();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealthFraction()
    {
        return healthPoints / startHealth;
    }

    // Saving System
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

  
}
