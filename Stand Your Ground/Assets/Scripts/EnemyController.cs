using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed = .5f;
    private float zbound = 3f;
    private float xbound = 2.5f;
    private GameObject player;
    public float health = 100f;

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
    }

    // Destroy the enemy
    public void Die()
    {
        Destroy(gameObject);
    }

}
