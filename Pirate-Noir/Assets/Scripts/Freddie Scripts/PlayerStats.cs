using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float BaseMoveSpeed = 7f;
    public float BaseSprintSpeed = 12f;

    public float SpeedMultiplier = 1.0f; // Multiplier for movement speed (e.g., from buffs/debuffs)
    public float MoveSpeed => BaseMoveSpeed * SpeedMultiplier; // Current move speed considering modifiers
    public float SprintSpeed => BaseSprintSpeed * SpeedMultiplier; // Current sprint speed considering modifiers

    [Header("Health")]
    public int Lives = 3; // Number of lives the player has
    public float CurrentHealth = 100; // Current health of the player
    public float MaxHealth = 100; // Maximum health of the player

    [Header("Stamina")]
    public float CurrentStamina = 100; // Current stamina of the player
    public float MaxStamina = 100; // Maximum stamina of the player

    [Header("Items")]
    public int GrapplingHooks = 0; // Number of grappling hooks (Charges?) the player has
    public int Bombs = 0; // Number of bombs the player has

    [Header("Inventory")]
    public int Keys = 0; // Number of keys the player has
    public int Coins = 0; // Number of coins the player has
    public int Score = 0; // Player's score (if we want to track it)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
