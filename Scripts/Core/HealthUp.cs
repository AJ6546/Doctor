using System.Collections;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    [SerializeField] float updatePercent;
    PoolManager poolManager;
    [SerializeField] string healthUpEffect;
    private void Start()
    {
        // Instance of Pool Manager
        poolManager = PoolManager.instance;
        // Random % for updating health
        updatePercent = Random.Range(10, 50);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Updates health of player on pickup
            other.GetComponent<Health>().UpdateHealth(updatePercent);
            StartCoroutine(Deactivate());  
        }
    }
    IEnumerator Deactivate()
    {
        // Play a particle effect when picked up
        poolManager.Spawn(healthUpEffect, transform.position, transform);
        yield return new WaitForSeconds(0.2f);
        // deactivate after a delay
        gameObject.SetActive(false);
    }
}
