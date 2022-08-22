using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float chasingDistance = 3f,attackingDistance=1f;
    [SerializeField] Health target;
    [SerializeField] EnemyFighter ef;
    [SerializeField] EnemyMover em;
    private void Start()
    {
        ef = GetComponent<EnemyFighter>(); // ref to enemy fighter
        em = GetComponent<EnemyMover>(); // ref to enemy mover
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>(); // ref to player health
    }
    private void Update()
    {
        // stop attacking if character is dead
        if (GetComponent<Health>().IsDead()) return;

        // always look at player while attacking them
        transform.LookAt(target.transform);

        // Do nothing when player is far
        if(GetDistance(target)>chasingDistance)
        {
            em.SetMove(false);
        }
        else
        {
            // if at attacking distance attack the player
            if (GetDistance(target) <= attackingDistance)
            {
                em.SetMove(false);
                ef.AttackBehaviour();
            }
            // if at chasing distance, chase the player
            else
            {
                em.SetMove(true);
            }
        }
    }

    // Returns distance between player and enemy
    public float GetDistance(Health target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }
}
