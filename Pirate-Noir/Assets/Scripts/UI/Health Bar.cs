using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    // function connecting player health to UI 

    public Slider healthBar;
    /*public int maxHealth = 100;
    public int health;*/
    public PlayerStats player; // to use the health values from that code

    public Transform Attack; // enemies themselves won't hurt you but they will have attacks that do
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.CurrentHealth = player.MaxHealth;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != player.CurrentHealth)
        {
            healthBar.value = player.CurrentHealth;
        }
    }

}