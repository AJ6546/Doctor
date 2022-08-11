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
        cdTimer = GetComponent<CooldownTimer>();
        uiAssigner = GetComponent<UIAssigner>();
    }

    void Update()
    {
        enemies = GetAllEnemies();
        Refill();
        if(enemies.Count>0)
            target = FindNearestEnemy().GetComponent<Health>();
        if (GetComponent<Health>().IsDead()) return;
        Attack();
    }

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
    private float FindDistance(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }
    #region Attack
    void Attack()
    {
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
            playerAnimator.SetTrigger("Attack01");
            cdTimer.nextAttackTime["Attack01"] = cdTimer.coolDownTime["Attack01"] + (int)Time.time;
            attack01Fill.GetComponent<Image>().fillAmount = 1;
            damage = strength / 5;
            attackSoundEffect = "Whoosh";
        }
        if (cdTimer.nextAttackTime["Attack02"]
       < Time.time && (Input.GetKeyDown(attack2) || attack02Button.Pressed))
        {
            playerAnimator.SetTrigger("Attack02");
            cdTimer.nextAttackTime["Attack02"] = cdTimer.coolDownTime["Attack02"] + (int)Time.time;
            attack02Fill.GetComponent<Image>().fillAmount = 1;
            damage = strength / 3;
            attackSoundEffect = "Whoosh";
        }
        if (cdTimer.nextAttackTime["Attack03"]
       < Time.time && (Input.GetKeyDown(attack3) || attack03Button.Pressed))
        {
            playerAnimator.SetTrigger("Attack03");
            cdTimer.nextAttackTime["Attack03"] = cdTimer.coolDownTime["Attack03"] + (int)Time.time;
            attack03Fill.GetComponent<Image>().fillAmount = 1;
            damage = strength / 2;
            attackSoundEffect = "Swoosh";
        }
        if (cdTimer.nextAttackTime["Attack04"]
       < Time.time && (Input.GetKeyDown(attack4) || attack04Button.Pressed))
        {
            playerAnimator.SetTrigger("Attack04");
            cdTimer.nextAttackTime["Attack04"] = cdTimer.coolDownTime["Attack04"] + (int)Time.time;
            attack04Fill.GetComponent<Image>().fillAmount = 1;
            damage = strength;
            attackSoundEffect = "Swoosh";
        }
    }
    void Hit(string sfx)
    {
        audioMansger.Play(attackSoundEffect, transform.position);
        if (InAttackRange())
        {
            transform.LookAt(target.transform);
            target.TakeDamage(gameObject, damage+damageModifier);
            audioMansger.Play(sfx, transform.position);
        }
    }
    public void SetDamageModifier(float damageModifier)
    {
        this.damageModifier = damageModifier;
    }
    bool InAttackRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }
    #endregion
    #region Refill
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
