using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    // function connecting player health to UI 

    public Slider healthBar;
    public Slider EaseHealthBar;
    public float lerpSpeed = 0.05f;

    /*public int maxHealth = 100;
    public int health;*/
    public PlayerStats player; // to use the health values from that code


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.CurrentHealth = player.MaxHealth;

        healthBar.maxValue = player.MaxHealth;
        EaseHealthBar.maxValue = player.MaxHealth;

        healthBar.value = player.MaxHealth;
        EaseHealthBar.value = player.MaxHealth;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != player.CurrentHealth)
        {
            healthBar.value = player.CurrentHealth;
        }

        if (healthBar.value != EaseHealthBar.value)
        {
            EaseHealthBar.value = Mathf.Lerp(EaseHealthBar.value, player.CurrentHealth, Time.deltaTime * lerpSpeed); // gives cool souls like effect when losing health
            // health lost will have an extra yellow bar, which will then disappear easing into the new current health.
        }
    }

}