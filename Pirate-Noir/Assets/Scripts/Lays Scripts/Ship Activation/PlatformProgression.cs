using UnityEngine;

public class PlatformProgression : MonoBehaviour
{
    [SerializeField] private int killsNeeded = 2;
    [SerializeField] private GameObject nextObjectToActivate;

    [SerializeField] private Transform platformToRotate;

    [SerializeField] private float rotationSpeed = 10f;

    private int currentKills = 0;
    private bool activated = false;
    private bool rotatePlatform = false;

    private PlatformCount[] enemies;

private void Start()
{
    Debug.Log("Kills start = " + currentKills);
    enemies = FindObjectsByType<PlatformCount>(FindObjectsSortMode.None);
}
    public void EnemyDefeated()
    {
        if (activated)
            return;

        currentKills++;

    if (currentKills >= killsNeeded)
    {
        activated = true;
        nextObjectToActivate.SetActive(true);
        rotatePlatform = true;

        foreach (var enemy in enemies)
        {
            enemy.ActivatePhysics();
        }
    }
    }

    private void Update()
    {
        if (rotatePlatform)
        {
            Vector3 currentRotation = platformToRotate.eulerAngles;

            float xRotation = currentRotation.x;

            // Convert Unity angle to negative range
            if (xRotation > 180f)
                xRotation -= 360f;

            // Stop at -35
            if (xRotation > -60f)
            {
                platformToRotate.Rotate(-rotationSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                rotatePlatform = false;

                Vector3 finalRotation = platformToRotate.eulerAngles;
                finalRotation.x = -35f;

                platformToRotate.eulerAngles = finalRotation;
            }
        }
    }
}