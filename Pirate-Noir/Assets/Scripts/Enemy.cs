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
    /*public Animator animator; // to play attack animations, will be used in later versions
    public bool isAttacking = false; // to prevent the enemy from attacking multiple times in a row without cooldown, will be used in later versions*/

    public float DetectRange = 8f; // the range from which the enemy will detect and attack the player.
    public float FarRange = 15f;
    public float distance;

    [Header("Behavior Settings")]
    public bool AttackPhase = false;
    public float AttackPhaseRange = 5f;
    public float BehaviorTimer = 5f;

    [Header("Roam Settings")]
    public float roamRadius = 10f; 
    public float roamTimer = 5f; // time in seconds between each roam action
    private float roamTime;
    public Vector3 roamPosition;

    public EnemyState currentState; // to determine the current state of the enemy, will be used in later versions to make the enemy do different things based on the state.
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sword.SetActive(false); // sword is disabled at the start, will be enabled when attacking, this will probably change in later versions
        currentState = EnemyState.Idle; // enemy starts in idle state, this will probably change in later versions
    }

    // Enemy will have two "phases", the roam and the attack phase
    /*
    Enemy roam will have:
        - Idle
        - Roam
        - Chase
    
    Enemy will reach attack phase when player is within the specified range, having:
        - Strafe
        - Attack
        - Approach (might make this one connect TO the attack)
        - Step back

    I also want to find a way in which enemies have a smaller chance of repeating an action the more they do it.

    */

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, Player.position); // distance which will be used to measure and trigger enemy attacks

        //agent.SetDestination(Player.position);
        
        
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Roam:
                Roam();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Strafe:
                // code to make the enemy circle around the player, will be used in later versions
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
            
        }

        if(distance <= AttackPhaseRange)
        {
            AttackPhase = true;
        }

        else if(distance > AttackPhaseRange)
        {
            AttackPhase = false;
        }
        
        

    }

    public IEnumerator AttackCooldown()
    {
        Sword.SetActive(false); // sword disappears when in cooldown, this will probably change in later versions
        yield return new WaitForSeconds(attackCooldown);

        // Sword.SetActive(true); // will make a function later to make the player be detected after entering a certain range, making the sword spawn in.
    }

    public void Idle()
    {
        // idle animation probably goes here.

        BehaviorChoice(); // will choose a behavior after a certain amount of time, this will probably change in later versions
        
        if(distance <= FarRange)
        {
            currentState = EnemyState.Chase; // if player is within far range, enemy will start chasing him, this will be used in later versions to make the enemy do different things based on the state.
        }
    }
    
    public void Roam()
    {
        // code to make the enemy walk around in a few selected areas at random from a specific distance from the enemy, will be used in later versions
        roamTime += Time.deltaTime;

        // If enough time passed OR reached destination
        if (roamTime >= roamTimer || Vector3.Distance(transform.position, roamPosition) < 2f)
        {
            roamPosition = RandomDirection();

            agent.SetDestination(roamPosition);

            roamTime = 0f;
        }
        
        BehaviorChoice(); // will choose a behavior after a certain amount of time, this will probably change in later versions

        if(distance <= FarRange)
        {
            currentState = EnemyState.Chase; // Same thing as the idle .
        }
    }

    Vector3 RandomDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius; // direction is random, but limited within the radius.

        randomDirection += transform.position; 

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
    
    public void ChasePlayer()
    {
        // once player is detected, follow him, duh
        agent.SetDestination(Player.position);

        if (distance <= DetectRange) // player is closer, enemy is faster
        {
            agent.speed = speed;
        }
    }

    public void AttackPlayer()
    {
        // code to make enemy attack player, due to a lack of animation:
        // enemy will, for now, rush towards the player
        Sword.SetActive(true);

    }
    
    public void DamagePlayer()
    {

        
        
        StartCoroutine(AttackCooldown());
        // Play attack animation or sound here if needed
        player.CurrentHealth -= damage;
    }

    public enum EnemyState
    {
        Idle, // literally does nothing, just stands in idle.
        Roam, // walks around in a few selected areas at random from a specific distance from the enemy.
        Chase,
        Attack,
        Attack2,
        Attack3, // Enemies might possess more attacks in later versions, this is just a placeholder for now.
        Strafe, // circles around the enemy
        Approach,
        StepBack

    }

    public void BehaviorChoice()
    {
        // If the player goes far enough, make it so that after a few seconds, behavior resets to roam and/or idle.
        // This is temporary, I will change this over the course of development.

        int choice = Random.Range(0, 100); // Percentage chance to choose a behavior.

        if(!AttackPhase)
        {
            if (choice < 33)
            {
                currentState = EnemyState.Idle;
            }
            else if (choice < 66)
            {
                currentState = EnemyState.Roam;
            }
            
            
            /*else
            {
                currentState = EnemyState.Chase;
            }
            
            Enemy will only start chasing when player is in range, it will not trigger automatically.
            */


        }

        else if(AttackPhase)
        {
            if (choice < 25)
            {
                currentState = EnemyState.Strafe;
            }
            else if (choice < 50)
            {
                currentState = EnemyState.Attack;
            }
            else if (choice < 75)
            {
                currentState = EnemyState.Approach;
            }
            else
            {
                currentState = EnemyState.StepBack;
            }
        }

        

        
        
        
    }

    
}
