using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    // function connecting player health to UI 

    public Image healthBar;
    public Image EaseHealthBar;
    public GameObject HealthUI;
    public float lerpSpeed = 0.05f;

    /*public int maxHealth = 100;
    public int health;*/
    public PlayerStats player; // to use the health values from that code
    public PauseManagement PauseManag;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerStats>(); // Get the PlayerStats component
        GameObject mainBarObj = GameObject.Find("HealthBar");
        GameObject easeBarObj = GameObject.Find("HurtBar");

        PauseManag = Object.FindAnyObjectByType<PauseManagement>();

        if (mainBarObj != null) healthBar = mainBarObj.GetComponent<Image>();
        if (easeBarObj != null) EaseHealthBar = easeBarObj.GetComponent<Image>();


        
        player.CurrentHealth = player.MaxHealth;

        healthBar.fillAmount = 1f;
        EaseHealthBar.fillAmount = 1f;

        
    }

    // Update is called once per frame
    void Update()
    {
        float targetFill = (float)player.CurrentHealth / player.MaxHealth;

        if (healthBar.fillAmount != targetFill)
        {
            healthBar.fillAmount = targetFill;
        }

        if (EaseHealthBar.fillAmount != healthBar.fillAmount)
        {
            EaseHealthBar.fillAmount = Mathf.Lerp(EaseHealthBar.fillAmount, targetFill, Time.deltaTime * lerpSpeed); // gives cool souls like effect when losing health
            // health lost will have an extra yellow bar, which will then disappear easing into the new current health.
        }
    }

    

}