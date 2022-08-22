using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] Canvas gameOverScreen;
    [SerializeField] Health playerHealth;

    // Used to display pop up when player health is <= 0. 
    void Start()
    {
        // Deactivating GameOver screen at the beginning of a scene
        gameOverScreen.gameObject.SetActive(false);
        // Finding the GameObject with tag "Player".
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        if(playerHealth.IsDead())
        {
            // Activating GameOver screen when player is dead
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
