using UnityEngine;

public class PlatformCount : MonoBehaviour
{
    public PlatformProgression platformManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Die();
        }
    }

    public void Die()
    {
        platformManager.EnemyDefeated();

        Destroy(gameObject);
    }
}