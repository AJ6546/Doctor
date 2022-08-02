using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] Health target;
    [SerializeField] GameObject selectedTarget;
    [SerializeField] float targetReach = 20f;
    [SerializeField] List<GameObject> npcs = new List<GameObject>();
    [SerializeField] List<GameObject> npcsAndEnemies = new List<GameObject>();
    [SerializeField] int state = 1;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    void Update()
    {
        enemies = GetAllEnemies();
        npcs = GetAllNPCs();
        npcsAndEnemies.AddRange(npcs);
        npcsAndEnemies.AddRange(enemies);
        if (state == 0)
        {
            if ((selectedTarget == null || FindDistance(selectedTarget) >= targetReach) && FindNearestEnemy() != null)
                target = FindNearestEnemy().GetComponent<Health>();
            else target = selectedTarget.GetComponent<Health>();
        }
        else if (state == 1) // Enemies Target NPCs and Players
        {
            target = FindNearestNPCOrPlayer().GetComponent<Health>();
        }
        else  //NPCs Target Enemies
        {
            target = FindNearestEnemy().GetComponent<Health>();
        }
    }

    #region GetEnemy
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
    private GameObject FindNearestEnemyOrPlayer()
    {
        float minDist = FindDistance(GameObject.FindGameObjectWithTag("Player"));
        GameObject nearestEnemyorPlayer = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject enemy in enemies)
        {
            float dist = FindDistance(enemy);
            if (minDist > dist)
            {
                nearestEnemyorPlayer = enemy;
                minDist = dist;
            }
        }
        return nearestEnemyorPlayer;
    }
    #endregion
    #region GetNPC
    private List<GameObject> GetAllNPCs()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject ec in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (!ec.GetComponent<Health>().IsDead())
                temp.Add(ec);
        }
        return temp;
    }
    private GameObject FindNearestNPC()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestNPC = null;
        foreach (GameObject npc in npcs)
        {
            float dist = FindDistance(npc);
            if (minDist > dist)
            {
                nearestNPC = npc;
                minDist = dist;
            }
        }
        return nearestNPC;
    }
    private GameObject FindNearestNPCOrPlayer()
    {
        float minDist = FindDistance(GameObject.FindGameObjectWithTag("Player"));
        GameObject nearestNPCorPlayer = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject npc in npcs)
        {
            float dist = FindDistance(npc);
            if (minDist > dist)
            {
                nearestNPCorPlayer = npc;
                minDist = dist;
            }
        }
        return nearestNPCorPlayer;
    }
    #endregion
    #region EnemiesAndNPCs
    private GameObject FindNearestNPCOrEnemy()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestNPCOrEnemy = null;
        foreach (GameObject npcOrEnemy in npcsAndEnemies)
        {
            float dist = FindDistance(npcOrEnemy);
            if (minDist > dist && npcOrEnemy != gameObject)
            {
                nearestNPCOrEnemy = npcOrEnemy;
                minDist = dist;
            }
        }
        return nearestNPCOrEnemy;
    }
    #endregion
    private float FindDistance(GameObject targt)
    {
        return Vector3.Distance(targt.transform.position, transform.position);
    }
    public Health GetTarget()
    {
        return target;
    }
}
