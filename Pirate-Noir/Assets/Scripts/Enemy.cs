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

    public float DetectRange = 8f; // the range from which the enemy will detect and attack the player.
    public float distance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, Player.position); // distance which will be used to measure and trigger enemy attacks

        if(distance <= DetectRange)
        {
            agent.SetDestination(Player.position);
        }
        
        
    }

    public IEnumerator AttackCooldown()
    {
        Sword.SetActive(false); // sword disappears when in cooldown, this will probably change in later versions
        yield return new WaitForSeconds(attackCooldown);

        Sword.SetActive(true); // will make a function later to make the player be detected after entering a certain range, making the sword spawn in.
    }

    public void ChasePlayer()
    {
        // once player is detected, follow him, duh
        agent.SetDestination(Player.position);
    }
    
    public void AttackPlayer()
    {

        Sword.SetActive(true);
        
        StartCoroutine(AttackCooldown());
        // Play attack animation or sound here if needed
        player.CurrentHealth -= damage;
    }

    

    
}
