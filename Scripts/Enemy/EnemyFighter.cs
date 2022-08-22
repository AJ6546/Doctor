using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{
    Animator animator;
    Health target;
   
    CooldownTimer cdTimer;
    [SerializeField] bool canAttack=true;
    [SerializeField] float damagingDistance = 1f, strength=10, damage;
    [SerializeField] AudioManager audioManager;
    [SerializeField] string attackSoundEffect="Grawl";
    private void Start()
    {
        audioManager = AudioManager.instance;
        animator = GetComponent<Animator>();
        cdTimer = GetComponent<CooldownTimer>();
        // Enemy always targets player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    #region EnemyAttack
    public void AttackBehaviour()
    {
        // Don't attack if player is already dead
        if (target.IsDead())
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            return;
        }
        
        if(Time.time > cdTimer.nextAttackTime["Attack01"] && canAttack)
        {
            // play animation 
            animator.SetTrigger("Attack01");
            // update next attack time
            cdTimer.nextAttackTime["Attack01"] = (int)Time.time + cdTimer.coolDownTime["Attack01"];
            // cannot do a different attack while still animating
            canAttack = false;
            // damage this attack causes the player
            damage = strength / 5;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack02"] && canAttack)
        {
            // play animation 
            animator.SetTrigger("Attack02");
            // update next attack time
            cdTimer.nextAttackTime["Attack02"] = (int)Time.time + cdTimer.coolDownTime["Attack02"];
            // cannot do a different attack while still animating
            canAttack = false;
            // damage this attack causes the player
            damage = strength / 3;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack03"] && canAttack)
        {
            // play animation 
            animator.SetTrigger("Attack03");
            // update next attack time
            cdTimer.nextAttackTime["Attack03"] = (int)Time.time + cdTimer.coolDownTime["Attack03"];
            // cannot do a different attack while still animating
            canAttack = false;
            // damage this attack causes the player
            damage = strength / 5;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack04"] && canAttack)
        {
            // play animation 
            animator.SetTrigger("Attack04");
            // update next attack time
            cdTimer.nextAttackTime["Attack04"] = (int)Time.time + cdTimer.coolDownTime["Attack04"];
            // cannot do a different attack while still animating
            canAttack = false;
            // damage this attack causes the player
            damage = strength;
        }
    }

    #endregion

    // Method triggered from animation
    void Hit(string sfx)
    {
        // Play sfx for this attack
        audioManager.Play(attackSoundEffect, transform.position);

        
        if (AtDamagingDistance())
        {
            // If at attacking distance, damage
            target.TakeDamage(gameObject, damage);
            // Play sfx on attack hitting the player
            audioManager.Play(sfx, transform.position);
        }
        canAttack = true;
    }

    // Returns true if the player is within attackable distance, else false
    public bool AtDamagingDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        return distanceToTarget <= damagingDistance;
    }

    // Visualizing attackable distance.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, damagingDistance);
    }

}
