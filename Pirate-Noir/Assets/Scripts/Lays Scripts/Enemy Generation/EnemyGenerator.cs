using UnityEngine;
// Nav-Mesh
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Player Target")]
    public Transform playerTransform;


[Header("Loot Settings")]
    [Tooltip("Add your various loot chests or items here.")]
    public GameObject[] lootPrefabs;
    public int minLootCount = 2;
    public int maxLootCount = 6;
    [Tooltip("The Y-axis rotation angles you want your loot to face (e.g., 0 = North, 90 = East, 180 = South, 270 = West).")]
    public float targetYRotation = 0f;

    // --- ADD THIS TO THE TOP OF YOUR SCRIPT WITH THE OTHER VARIABLES ---
[Header("Loot Snapping Settings")]
    [Tooltip("Set this to the specific layer your floor/ground objects are on.")]
    public LayerMask floorLayer;
    [Tooltip("Slight upward adjustment to pull the loot out of the ground if its pivot point is centered.")]
public float lootYOffset = 0.5f;



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

        // 2. Spawning Loot (Independent from NavMesh)
        int lootToSpawn = Random.Range(minLootCount, maxLootCount + 1); // +1 because integer max is exclusive
        Debug.Log("Spawning " + lootToSpawn + " loot items");
        SpawnLoot(lootToSpawn);

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

private void SpawnLoot(int amount)
{
    if (lootPrefabs == null || lootPrefabs.Length == 0)
    {
        Debug.LogWarning("No loot prefabs assigned in the Loot Prefabs array!");
        return;
    }

    for (int i = 0; i < amount; i++)
    {
        // 1. Pick a random horizontal spot
        float x = Random.Range(minBounds.x, maxBounds.x);
        float z = Random.Range(minBounds.z, maxBounds.z);

        // 2. Fire the laser down from safely above the upper bounds
        float highYStart = maxBounds.y + 20f;
        Vector3 raycastOrigin = new Vector3(x, highYStart, z);

        RaycastHit hit;
        float maxRayDistance = (highYStart - minBounds.y) + 30f;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, maxRayDistance, floorLayer))
        {
            // 3. Get the raw floor point, then add your custom offset to the Y axis
            Vector3 calculatedPosition = hit.point;
            calculatedPosition.y += lootYOffset;

            // 4. Randomly pick an item from your array
            int randomLootIndex = Random.Range(0, lootPrefabs.Length);
            GameObject chosenLootPrefab = lootPrefabs[randomLootIndex];

            if (chosenLootPrefab != null)
            {
                Quaternion specificRotation = Quaternion.Euler(0f, targetYRotation, 0f);

                // Spawn at the calculated position with the offset applied
                Instantiate(chosenLootPrefab, calculatedPosition, specificRotation);
            }
        }
        else
        {
            Debug.LogWarning($"Loot Raycast missed the floor at X: {x}, Z: {z}. No collider on the designated layer was found.");
        }
    }
}
}
