using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    [Header("Zombie Stats")]
    // private float speed = .5f;
    // private string zName;
    // private float zbound = 3f;
    // private float xbound = 2.5f;
    // public float health;
    // public float attack;
    // public float spawnProbability;

    [Header("Animator")]
    public Animator animator;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();

        Debug.Log("Enemy spawn probability: " + enemyData.spawnProbability);
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
        transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, enemyData.speed * Time.deltaTime);

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
        enemyData.health -= damage;
        Debug.Log (name + " took " + damage + " damage and has " + enemyData.health + " health left.");
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
            // Set the enemy's stats
            float health = enemyData.health;
            Debug.Log(name + " and their health: " + health);
            float speed = enemyData.speed;
            float attack = enemyData.attack;
            float spawnProbability = enemyData.spawnProbability;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player"){
            animator.SetBool("Attacking", true);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Player"){
            animator.SetBool("Attacking", false);
        }
    }

}
