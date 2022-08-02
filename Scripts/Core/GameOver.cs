using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] Canvas gameOverScreen;
    [SerializeField] Health playerHealth;
    void Start()
    {
        gameOverScreen.gameObject.SetActive(false);
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if(playerHealth.IsDead())
        {
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
