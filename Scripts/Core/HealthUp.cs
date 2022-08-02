using System.Collections;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    [SerializeField] float updatePercent;
    PoolManager poolManager;
    [SerializeField] string healthUpEffect;
    private void Start()
    {
        poolManager = PoolManager.instance;
        updatePercent = Random.Range(10, 50);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Health>().UpdateHealth(updatePercent);
            StartCoroutine(Deactivate());
            
        }
    }
    IEnumerator Deactivate()
    {
        poolManager.Spawn(healthUpEffect, transform.position, transform);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
