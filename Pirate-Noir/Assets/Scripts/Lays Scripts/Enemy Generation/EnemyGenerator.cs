using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    // On the final ship, we want to spawn more enemies.
    // This line will get the value of the finalPlatform variable from the PlatformProgression script, which is true if this is the final ship.
   [SerializeField] private PlatformProgression platformProgression;

    // There will always be at least 4 enemies
    private int minEnemies = 4;

    private void Start()
    {
        if (platformProgression.FinalPlatform)
        {
            minEnemies += 2; // 4 becomes 6 on the last ship
        }

        // Randomly spawn between minEnemies and 10 enemies
        int enemiesToSpawn = Random.Range(minEnemies, 11);

        Debug.Log("Spawning " + enemiesToSpawn + " enemies");

        // Call the method to spawn enemies
        SpawnEnemies(enemiesToSpawn);
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Instantiate enemy here
        }
    }
}
