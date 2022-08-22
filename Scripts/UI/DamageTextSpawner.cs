using UnityEngine;

// Spawns a damage text over the top of character's head to display damage they are taking 
public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] DamageText damageText;
    public void Spawn(float damage)
    {
        if (damage > 0)
        {
            DamageText instance = Instantiate<DamageText>(damageText, transform);
            instance.SetDamageText((int)(damage));
        }
    }
}
