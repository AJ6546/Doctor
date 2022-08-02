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
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    private void Update()
    {
        //if (AtDamagingDistance())
        //{
        //    transform.LookAt(target.transform);
        //    AttackBehaviour(); }
    }
    #region EnemyAttack
    public void AttackBehaviour()
    {
        if (target.IsDead())
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            return;
        }
        
        if(Time.time > cdTimer.nextAttackTime["Attack01"] && canAttack)
        {
            animator.SetTrigger("Attack01");
            cdTimer.nextAttackTime["Attack01"] = (int)Time.time + cdTimer.coolDownTime["Attack01"];
            canAttack = false;
            damage = strength / 5;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack02"] && canAttack)
        {
            animator.SetTrigger("Attack02");
            cdTimer.nextAttackTime["Attack02"] = (int)Time.time + cdTimer.coolDownTime["Attack02"];
            canAttack = false;
            damage = strength / 3;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack03"] && canAttack)
        {
            animator.SetTrigger("Attack03");
            cdTimer.nextAttackTime["Attack03"] = (int)Time.time + cdTimer.coolDownTime["Attack03"];
            canAttack = false;
            damage = strength / 5;
        }
        if (Time.time > cdTimer.nextAttackTime["Attack04"] && canAttack)
        {
            animator.SetTrigger("Attack04");
            cdTimer.nextAttackTime["Attack04"] = (int)Time.time + cdTimer.coolDownTime["Attack04"];
            canAttack = false;
            damage = strength;
        }
    }

    #endregion
    void Hit(string sfx)
    {
        audioManager.Play(attackSoundEffect, transform.position);
        if (AtDamagingDistance())
        {
            target.TakeDamage(gameObject, damage);
            audioManager.Play(sfx, transform.position);
        }
        canAttack = true;
    }
    public bool AtDamagingDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        return distanceToTarget <= damagingDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, damagingDistance);
    }

}
