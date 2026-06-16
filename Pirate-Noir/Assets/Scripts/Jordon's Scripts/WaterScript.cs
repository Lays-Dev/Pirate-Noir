using UnityEngine;

public class WaterScript : MonoBehaviour
{
    
        public string playerTag = "Player";
    public float PainDelay = 1f; //how often the player loses health
    public float PainAmount = 12.5f; //how much health the player loses each time they take damage from the water, this will probably change in later versions
    public PlayerStats playerStats; // reference to the player's stats script to modify health
    private float damageTimer;

    void Start()
    {
        playerStats = Object.FindAnyObjectByType<PlayerStats>(); // reference to the player's stats script to modify health
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Reduce the timer by the time passed since the last frame
            damageTimer -= Time.deltaTime;

            // When the timer hits 0, deal damage and reset the timer
            if (damageTimer <= 0)
            {
                Debug.Log("Player has fallen into the water");
                playerStats.CurrentHealth -= PainAmount;
                
                // Reset the timer
                damageTimer = PainDelay;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object has the Player tag
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player is back on dry land");
            // ADD YOUR EXIT ACTION HERE
            //maybe water effect for leaving water if we have grapple possible?
            damageTimer = 0;
        }
    }
}
