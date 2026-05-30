using UnityEngine;

public class PlatformProgression : MonoBehaviour
{
#region Inspector
    [Header("Platform Progression Settings")]
    // Number of kills needed per ship to activate the next ship
    [SerializeField] private int killsNeeded = 2;
    // The next ship that will come onto the scene after the required kills are met
    [SerializeField] private GameObject nextObjectToActivate;
    // The current ship game object
    [SerializeField] private Transform platformToRotate;
    // Speed at which the ship rotates when activated
    [SerializeField] private float rotationSpeed = 1f;

#endregion

    // Keeps track of the number of kills the player has achieved
    private int currentKills = 0;
    // Prevents the ship from being activated multiple times
    private bool activated = false;
    // Controls whether the ship should rotate after activation
    private bool rotatePlatform = false;
    // Stores references to every enemy with the PlatformCount script
    private PlatformCount[] enemies;

private void Start()
{
    // Kills start at 0

    Debug.Log("Kills start = " + currentKills);
    // Finds all gambojects with the PlatformCount script. This will just be the enemies. Stores them in an array ( enemies)
    enemies = FindObjectsByType<PlatformCount>(FindObjectsSortMode.None);
}

// Called when an enemy dies. Another script can call this because it is public.
    public void EnemyDefeated()
    {
        // If the next ship has already been activated.
        if (activated)
            return;

        // Increase kill count by 1
        currentKills++;

    // Have enough enemies dies to activate the next ship?
    if (currentKills >= killsNeeded)
    {
        // Prevents this from being called twice.
        activated = true;
        // Turns on the game object, the next ship
        nextObjectToActivate.SetActive(true);
        // Starts rotating the ship
        rotatePlatform = true;

        // Loops through each enemy in the array.
        foreach (var enemy in enemies)
        {
            // Runs once for each enemy
            // Allows the enemies to fall
            enemy.ActivatePhysics();
        }
    }
    }

// Runs every frame
    private void Update()
    {

        // Run this if the ship is currently rotating
        if (rotatePlatform)
        {
            // Ships current rotation stored in currentRotation
            Vector3 currentRotation = platformToRotate.eulerAngles;

            float xRotation = currentRotation.x;


            // Unity Rotations are stored in 360 degrees. -60 degrees = 300 degrees.
            // Conversion
            if (xRotation > 180f)
                xRotation -= 360f;

            // Stop rotating at -60 degrees.
            if (xRotation > -60f)
            {
                // X axis
                platformToRotate.Rotate(-rotationSpeed * Time.deltaTime, 0f, 0f);
            }
            // Stop rotation 
            else
            {
                // Stops update
                rotatePlatform = false;
                // Current rotation
                Vector3 finalRotation = platformToRotate.eulerAngles;
                // Snaps to angle
                finalRotation.x = -60f;

                platformToRotate.eulerAngles = finalRotation;
            }
        }
    }
}