using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{
    // every enemy script will inherit from this big boy

    public int health;
    public int damage;
    public float speed;

    public NavMeshAgent agent;
    public Transform Player;
    public PlayerStats player; // to use the health values from that code

    
    [Header("Attack Settings")]
    public Transform Attack; // enemies themselves won't hurt you but they will have attacks that do
    public GameObject Sword; // Model that will get disabled after attack and enabled when attacking.
    public float attackCooldown = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(Player.position); // Chase the player
    }

    public IEnumerator AttackCooldown()
    {
        Sword.SetActive(false); // sword disappears when in cooldown, this will probably change in later versions
        yield return new WaitForSeconds(attackCooldown);

        Sword.SetActive(true); // will make a function later to make the player be detected after entering a certain range, making the sword spawn in.
    }
    
    public void AttackPlayer()
    {
        
        Sword.SetActive(true);
        StartCoroutine(AttackCooldown());
        // Play attack animation or sound here if needed
        player.CurrentHealth -= damage;
    }

    

    
}
