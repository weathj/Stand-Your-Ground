using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed = .5f;
    public string zName;
    private float zbound = 3f;
    private float xbound = 2.5f;
    private GameObject player;
    public float health = 100f;
    public EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        Move();

        // Rotate towards the player
        Rotate();
    }

    // Locate player position and move towards it
    void Move()
    {
        // Get the player position
        Vector3 playerPosition = player.transform.position;

        // Get the enemy position
        Vector3 enemyPosition = transform.position;

        // Get the distance between the player and the enemy
        float distance = Vector3.Distance(playerPosition, enemyPosition);

        // Move towards the player
        transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, speed * Time.deltaTime);

    }

    // Rotate the enemy to face the player
    void Rotate()
    {
        // Get the player position
        Vector3 playerPosition = player.transform.position;

        // Get the enemy position
        Vector3 enemyPosition = transform.position;

        // Get the distance between the player and the enemy
        float distance = Vector3.Distance(playerPosition, enemyPosition);

        // Rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPosition - enemyPosition), 0.1f);

        // Correct enemy rotation
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

    }

    // Lower the enemy's health when hit by a bullet
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log (name + " took " + damage + " damage and has " + health + " health left.");
    }

    // Destroy the enemy
    public void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy died.");
    }

    // If enemy data is set, set the enemy's stats to the data's stats
    private void OnEnable()
    {
        if (enemyData != null)
        {

            // Set the enemy to the enemy prefab if there is no enemy
            // clear the enemy's children if there is an enemy
            if (transform.childCount == 0)
            {
                SpawnEnemy();
            }
            else
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                SpawnEnemy();
            }

            // Set the enemy's stats
            name = enemyData.zName;
            health = enemyData.health;
            Debug.Log(name + " and their health: " + health);
            speed = enemyData.speed;
        }
    }

    private GameObject SpawnEnemy()
    {

        GameObject enemy = null;
        // Get the enemy position
        Vector3 enemyPosition = transform.position;

        // Get the enemy's rotation
        Quaternion enemyRotation = transform.rotation;
        enemyRotation.eulerAngles = new Vector3(0, enemyRotation.eulerAngles.y, 0);

        // Spawn the enemy
        enemy = Instantiate(enemyData.enemyPrefab, enemyPosition, enemyRotation);

        // Set the enemy's parent to the enemy controller
        enemy.transform.parent = transform;

        return enemy;

    }

}
