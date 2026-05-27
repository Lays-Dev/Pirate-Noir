using UnityEngine;

public class PlatformProgression : MonoBehaviour
{
    [SerializeField] private int killsNeeded = 2;
    [SerializeField] private GameObject nextObjectToActivate;

    private int currentKills = 0;
    private bool activated = false;

    public void EnemyDefeated()
    {
        if (activated)
            return;

        currentKills++;

        if (currentKills >= killsNeeded)
        {
            activated = true;

            nextObjectToActivate.SetActive(true);
        }
    }
}