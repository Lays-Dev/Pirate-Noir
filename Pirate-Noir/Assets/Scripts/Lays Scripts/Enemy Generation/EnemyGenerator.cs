using UnityEngine;
// Nav-Mesh
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Player Target")]
    public Transform playerTransform;


    [Header("Enemy Prefabs")]
    public GameObject enemyEasy;
    public GameObject enemyMedium;
    public GameObject enemyHard;

    [Header("Spawn Area Bounds")]
    public Vector3 minBounds;
    public Vector3 maxBounds;

    [Header("NavMesh Settings")]
    [Tooltip("How far from the random point Unity will search for a valid NavMesh floor.")]
    public float maxNavMeshSearchRange = 5f;

    // On the final ship, we want to spawn more enemies.
    // This line will get the value of the finalPlatform variable from the PlatformProgression script, which is true if this is the final ship.
    [Header("References")]
    [SerializeField] private PlatformProgression finalPlatform;

    // There will always be at least 4 enemies
    private int minEnemies = 4;

    private void Start()
    {
        // Make sure finalPlatform is assigned in the inspector
        if (finalPlatform == null)
        {
            Debug.LogError("finalPlatform reference is not assigned in the inspector. Assign it in the Enemy Generator Manager GameObject.");
            return;
        }

        // Minimum enemies increases to 6 on the last ship
        if (finalPlatform.FinalPlatform)
        {
            minEnemies += 2;
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
            // Calculate a random coordinate
            float x = Random.Range(minBounds.x, maxBounds.x);
            float y = Random.Range(minBounds.y, maxBounds.y);
            float z = Random.Range(minBounds.z, maxBounds.z);
            Vector3 rawRandomPosition = new Vector3(x, y, z);

            // Get NavMesh position for the random point
            if (TryGetNavMeshPosition(rawRandomPosition, out Vector3 validNavMeshPosition))
            {
                if (enemyEasy != null)
                {
                    // Spawn the object
                    GameObject spawnedEnemy = Instantiate(enemyEasy, validNavMeshPosition, Random.rotation);

                    // Grab the Enemy script component off the newly spawned object
                    Enemy enemyScript = spawnedEnemy.GetComponent<Enemy>();

                    if (enemyScript != null)
                    {
                        // Inject the player reference directly into the enemy!
                        enemyScript.Player = playerTransform;
                    }
                }
            }
            else
            {
                // Optional: If the point was outside the map, try this loop iteration again
                Debug.LogWarning("Generated point " + rawRandomPosition + " was too far from the NavMesh. Skipping or retrying.");
                // i--; // Uncomment this line if you want to force it to retry until it succeeds!
            }
        }
    }
    private bool TryGetNavMeshPosition(Vector3 targetPosition, out Vector3 finalPosition)
    {
        // NavMeshHit holds the data of the found location if successful
        NavMeshHit hit;

        // NavMesh.AllAreas tells Unity to look at any walkable surface type
        if (NavMesh.SamplePosition(targetPosition, out hit, maxNavMeshSearchRange, NavMesh.AllAreas))
        {
            finalPosition = hit.position; // Return the exact snapped floor position
            return true;
        }

        finalPosition = Vector3.zero;
        return false;
    }
}
