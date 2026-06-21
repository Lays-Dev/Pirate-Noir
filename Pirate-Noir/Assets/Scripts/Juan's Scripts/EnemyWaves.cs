using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaves : MonoBehaviour
{
    // for all intents and purposes this is the spawner
    public Transform Player;
    public float Distance;


    public float SpawnDistance = 10f; // distance at which the enemy waves will start
    public float SpawnDistanceSqr; // squared distance for optimization, this will probably change in later versions
    public int roundNumber = 3;
    public int enemiesOnRound = 30;
    
    public int ActiveEnemies = 0;
    public int enemiesPerWave = 10;
    public int poolSize = 10;
    
    public GameObject regularEnemy;
    public GameObject rangedEnemy;
    public GameObject bigEnemy;
    public List<GameObject> enemyWaves = new List<GameObject>();

    public int currentRound = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnDistanceSqr = SpawnDistance * SpawnDistance; // calculate the squared distance for optimization

        for (int i = 0; i < poolSize; i++)
        {
            int enemyType = Random.Range(0, 3); // determine which type of enemy to spawn, this will probably change in later versions

            if (enemyType == 0)
            {
                GameObject obj = Instantiate(regularEnemy);
                obj.SetActive(false);
                enemyWaves.Add(obj);
            }
            else if (enemyType == 1)
            {
                GameObject obj2 = Instantiate(rangedEnemy);
                obj2.SetActive(false);
                enemyWaves.Add(obj2);
            }
            else if (enemyType == 2)
            {
                GameObject obj3 = Instantiate(bigEnemy);
                obj3.SetActive(false);
                enemyWaves.Add(obj3);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Distance = (this.transform.position - Player.position).sqrMagnitude; //get the distance from player, might have a more optimized version later.

        if (Distance < SpawnDistanceSqr)
        {
            WavesProcess();
        }
    }

    public GameObject GetPooledEnemy()
    {
        for (int i = 0; i < enemyWaves.Count; i++)
        {
            if (!enemyWaves[i].activeInHierarchy)
            {
                return enemyWaves[i];
            }
        }
        return null; // if no pooled object is available, return null, this will probably change in later versions
    }

    public void WavesProcess()
    {
        if (ActiveEnemies > 0)
        {
            return;
        }

        if(currentRound >= roundNumber)
        {
            return;
        }

        SpawnWave();
        currentRound++;
    } 

    public void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            GameObject enemy = GetPooledEnemy();
            if (enemy != null)
            {
                enemy.transform.position = this.transform.position; // spawn the enemy at the spawner's position, this will probably change in later versions
                enemy.SetActive(true);
                ActiveEnemies++;
            }
        }
    }
}
