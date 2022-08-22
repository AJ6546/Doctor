using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    [SerializeField] KeyCode attack1 = KeyCode.Alpha1,
        attack2 = KeyCode.Alpha2,
        attack3 = KeyCode.Alpha3,
        attack4 = KeyCode.Alpha4,
        attack5 = KeyCode.Alpha5;
    CooldownTimer cdTimer;
    public Animator playerAnimator;
    [SerializeField] Health target;
    [SerializeField] float attackRange = 3f;
    [SerializeField] FixedButton attack01Button, attack02Button, attack03Button, attack04Button;
    [SerializeField] UIAssigner uiAssigner;
    [SerializeField]
    GameObject attack01Fill, attack02Fill, attack03Fill, attack04Fill;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] float strength=10, damage, damageModifier=0;
    [SerializeField] AudioManager audioMansger;
    [SerializeField] string attackSoundEffect;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        audioMansger = AudioManager.instance;
        cdTimer = GetComponent<CooldownTimer>(); // ref to cooldown timer
        uiAssigner = GetComponent<UIAssigner>(); // ref to ui assigner - assigns differnet buttons
    }

    void Update()
    {
        enemies = GetAllEnemies(); // list of all all the enemies
        Refill(); // cooldown timer effect on the button ui
        if(enemies.Count>0)
            target = FindNearestEnemy().GetComponent<Health>(); // finds nearest enemy and sets it as target
        // If Player is dead they cannot attack
        if (GetComponent<Health>().IsDead()) return;
        // If Player is Saving the game they cannot attack
        if (FindObjectOfType<OnlineSaveLoadManager>().IsSaving()) return;
        Attack();
    }

    // Returns a list with all the gameobjects with tag enemy
    private List<GameObject> GetAllEnemies()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject ec in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!ec.GetComponent<Health>().IsDead())
                temp.Add(ec);
        }
        return temp;
    }

    // Returns the gameobject for nearest enemy from player
    private GameObject FindNearestEnemy()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float dist = FindDistance(enemy);
            if (minDist > dist)
            {
                nearestEnemy = enemy;
                minDist = dist;
            }
        }
        return nearestEnemy;
    }

    // distance between player and enemy
    private float FindDistance(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }
    #region Attack
    void Attack()
    {
        // setting ui attack buttons
        if (attack01Button == null || attack02Button == null || attack03Button == null || attack04Button == null)
        {
            attack01Button = uiAssigner.GetFixedButtons()[6];
            attack02Button = uiAssigner.GetFixedButtons()[7];
            attack03Button = uiAssigner.GetFixedButtons()[8];
            attack04Button = uiAssigner.GetFixedButtons()[9];
        }
        if (cdTimer.nextAttackTime["Attack01"]
            < Time.time && (Input.GetKeyDown(attack1) || attack01Button.Pressed))
        {
            // play animation 
            playerAnimator.SetTrigger("Attack01");
            // update cooldown timer
            cdTimer.nextAttackTime["Attack01"] = cdTimer.coolDownTime["Attack01"] + (int)Time.time;
            // set ui cooldown time
            attack01Fill.GetComponent<Image>().fillAmount = 1;
            // damage this attack deals
            damage = strength / 5;
            // setting sfx 
            attackSoundEffect = "Whoosh";
        }
        if (cdTimer.nextAttackTime["Attack02"]
       < Time.time && (Input.GetKeyDown(attack2) || attack02Button.Pressed))
        {
            // play animation 
            playerAnimator.SetTrigger("Attack02");
            // update cooldown timer
            cdTimer.nextAttackTime["Attack02"] = cdTimer.coolDownTime["Attack02"] + (int)Time.time;
            // set ui cooldown time
            attack02Fill.GetComponent<Image>().fillAmount = 1;
            // damage this attack deals
            damage = strength / 3;
            // setting sfx 
            attackSoundEffect = "Whoosh";
        }
        if (cdTimer.nextAttackTime["Attack03"]
       < Time.time && (Input.GetKeyDown(attack3) || attack03Button.Pressed))
        {
            // play animation 
            playerAnimator.SetTrigger("Attack03");
            // update cooldown timer
            cdTimer.nextAttackTime["Attack03"] = cdTimer.coolDownTime["Attack03"] + (int)Time.time;
            // set ui cooldown time
            attack03Fill.GetComponent<Image>().fillAmount = 1;
            // damage this attack deals
            damage = strength / 2;
            // setting sfx 
            attackSoundEffect = "Swoosh";
        }
        if (cdTimer.nextAttackTime["Attack04"]
       < Time.time && (Input.GetKeyDown(attack4) || attack04Button.Pressed))
        {
            // play animation 
            playerAnimator.SetTrigger("Attack04");
            // update cooldown timer
            cdTimer.nextAttackTime["Attack04"] = cdTimer.coolDownTime["Attack04"] + (int)Time.time;
            // set ui cooldown time
            attack04Fill.GetComponent<Image>().fillAmount = 1;
            // damage this attack deals
            damage = strength;
            // setting sfx 
            attackSoundEffect = "Swoosh";
        }
    }
    void Hit(string sfx)
    {
        // play sfx for the attack
        audioMansger.Play(attackSoundEffect, transform.position);
        if (InAttackRange())
        {
            // look at the enemy
            transform.LookAt(target.transform);
            // enemy takes damage from attack
            target.TakeDamage(gameObject, damage+damageModifier);
            // play sfx for attack hitting target
            audioMansger.Play(sfx, transform.position);
        }
    }
    // updates damages nemy takes while player holds a weapon
    public void SetDamageModifier(float damageModifier)
    {
        this.damageModifier = damageModifier;
    }
    // if enemy is within range to attack
    bool InAttackRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }
    #endregion
    #region Refill

    // Refel cooldown timers in ui
    void Refill()
    {
        if (attack01Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack01Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cdTimer.coolDownTime["Attack01"]) * Time.deltaTime;
            attack01Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cdTimer.nextAttackTime["Attack01"] - (int)Time.time, 0)).ToString();
        }
        else { attack01Fill.GetComponentInChildren<Text>().text = ""; }
        if (attack02Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack02Fill.GetComponent<Image>().fillAmount -=
            (1.0f / cdTimer.coolDownTime["Attack02"]) * Time.deltaTime;
            attack02Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cdTimer.nextAttackTime["Attack02"] - (int)Time.time, 0)).ToString();
        }
        else { attack02Fill.GetComponentInChildren<Text>().text = ""; }
        if (attack03Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack03Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cdTimer.coolDownTime["Attack03"]) * Time.deltaTime;
            attack03Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cdTimer.nextAttackTime["Attack03"] - (int)Time.time, 0)).ToString();
        }
        else { attack03Fill.GetComponentInChildren<Text>().text = ""; }

        if (attack04Fill.GetComponent<Image>().fillAmount > 0)
        {
            attack04Fill.GetComponent<Image>().fillAmount -=
                (1.0f / cdTimer.coolDownTime["Attack04"]) * Time.deltaTime;
            attack04Fill.GetComponentInChildren<Text>().text =
                (Mathf.Max(cdTimer.nextAttackTime["Attack04"] - (int)Time.time, 0)).ToString();
        }
        else { attack04Fill.GetComponentInChildren<Text>().text = ""; }
    }
    #endregion
}
