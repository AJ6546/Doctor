using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float chasingDistance = 3f,attackingDistance=1f,speed=1f;
    [SerializeField] Health target;
    [SerializeField] EnemyFighter ef;
    [SerializeField] EnemyMover em;
    private void Start()
    {
        ef = GetComponent<EnemyFighter>();
        em = GetComponent<EnemyMover>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    private void Update()
    {
        if (GetComponent<Health>().IsDead()) return;
        transform.LookAt(target.transform);
        if(GetDistance(target)>chasingDistance)
        {
            em.SetMove(false);
        }
        else
        {
            if (GetDistance(target) <= attackingDistance)
            {
                em.SetMove(false);
                ef.AttackBehaviour();
            }
            else
            {
                em.SetMove(true);
            }
        }
    }
    public float GetDistance(Health target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }
}
