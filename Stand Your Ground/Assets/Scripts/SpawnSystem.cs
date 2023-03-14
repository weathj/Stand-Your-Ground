using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnSystem : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GameObject[] ammoCrates;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] powerups;

    [Header("Item parents")]
    [SerializeField] private Transform ammoCrateParent;
    [SerializeField] private Transform enemiesParent;

    [Header("Probability")]
    [SerializeField] private float minSpawnDistance = 10f; // Min distance between spawns
    [SerializeField] private float maxSpawnDistance = 20f; // Max distance between spawns
    [SerializeField] private float ammoSpawnChance = 0.2f; // Chance of spawning ammoCrate
    [SerializeField] private float powerupSpawnChance = 0.1f; // Chance of spawning powerup

    [Header("Spawn Area")]
    [SerializeField] private float spawnDistance = 10f; // Spawn distance in front of player
    [SerializeField] Bounds spawnArea; // Spawn area game object
    [SerializeField] private float spawnAreaHeight = 1.3f; // Spawn area height

    public EventManager eventManager; // Event manager

    private Transform playerTransform; // Player transfrom
    private float nextSpawnDistance; // Distance next object will be spawned

    public float distancePerDifficultyLevel = 100f; // Distance between difficulty levels
    public int baseDifficultyLevel = 1; // Base difficulty level
    public float difficultyLevelExponent = 1.2f; // Difficulty level exponent
    private int currentdifficultyLevel; // Current difficulty level

    private int enemyCount; // Zombie count
    [SerializeField] private TMP_Text enemiesCountText; // Zombie count text

    private int powerupCount; // Powerup count
    [SerializeField] private TMP_Text powerupsCountText; // Powerup count text



    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance); // Set inital spawn range

        // Set current difficulty level
        currentdifficultyLevel = baseDifficultyLevel;
    }

    // Update is called once per frame
    void Update()
    {

        // Count enemies
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesCountText.text = enemyCount.ToString();

        // Count powerups
        powerupCount = GameObject.FindGameObjectsWithTag("Powerup").Length;
        powerupsCountText.text = powerupCount.ToString();

        spawnArea.center = playerTransform.position + playerTransform.forward * spawnDistance; // Set spawn area center

        // Calculate distance between player and last spawned object
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if player has reached the next spawn distance
        if (distanceToPlayer >= nextSpawnDistance)
        {
            // SpawnObject(); // Spawn a new object
            nextSpawnDistance = distanceToPlayer + Random.Range(minSpawnDistance, maxSpawnDistance); // Calculate next spawn distance
        }

        // // if enemies gets to large, delete some
        // if (enemyCount < 80)
        // {
        //     // delete the first enemy
        //     Destroy(enemies[0]);
        // }
    }

    private void SpawnObject()
    {
        Debug.Log("Spawning object");
    
        // Determine whether to spawn an ammo crate
        if (Random.value < ammoSpawnChance)
        {
            int randomIndex = Random.Range(0, ammoCrates.Length);
            // Get a random position within the spawn area
            SpawnAmmo(randomIndex);
        }
        // Determine whether to spawn a power-up
        else if (Random.value < powerupSpawnChance)
        {
            int randomIndex = Random.Range(0, powerups.Length);
            SpawnPowerup(randomIndex);
        }
    }

    private void OnEnable()
    {
        eventManager.distanceReachedEvent.AddListener(HandleDistanceReachedEvent); // Subscribe to event
    }

    private void OnDisable()
    {
        eventManager.distanceReachedEvent.RemoveListener(HandleDistanceReachedEvent); // Unsubscribe from event
    }

    private void HandleDistanceReachedEvent(float distance)
    {
        SpawnEnemies(currentdifficultyLevel, enemies); // Spawn enemies
        // increase diificulity level by one
        currentdifficultyLevel = currentdifficultyLevel + 1;
        SpawnObject(); // Spawn object
    }

// add a comment to each line of code to explain what it does
    public void SpawnEnemies(int difficultyLevel, GameObject[] enemies)
    {
        Debug.Log("Spawning enemies");

        // Calculate the total probability of all the enemies in list
        float totalProbability = 0f;
        // Loop through all enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get enemy controller
            EnemyController enemy = enemies[i].GetComponent<EnemyController>();
            // Add enemy spawn probability to total probability
            totalProbability += enemy.enemyData.spawnProbability;
        }

        // Check if total probability is 0
        if (totalProbability == 0f)
        {
            Debug.LogError("Total probability of enemies is 0.");
            return;
        }

        // Calculate the probability of each enemy type based on the difficulty level
        List<float> probabilities = new List<float>();
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get enemy controller
            EnemyController enemy = enemies[i].GetComponent<EnemyController>();
            // Calculate adjusted probability
            float adjustedProbability = enemy.enemyData.spawnProbability * (1f + difficultyLevel / 10f);
            // Add adjusted probability to list
            probabilities.Add(adjustedProbability / totalProbability);
        }

        // Generate a random number between 0 and 1
        float randomNumber = Random.Range(0f, 1f);

        // Determine which type of enemy to spawn based on probability values
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            // Add the probability of the current enemy type to the cumulative probability
            cumulativeProbability += probabilities[i];
            // Check if the random number is less than the cumulative probability
            if (randomNumber <= cumulativeProbability)
            {
                // Spawn the enemy
                SpawnEnemy(enemies[i]);
            }
        }

        Debug.Log("Random number: " + randomNumber + " Cumulative probability: " + cumulativeProbability);
    }

    private void SpawnEnemy(GameObject enemy)
    {
        // Generate a random position within the spawn area
        Vector3 spawnPosition = new Vector3(Random.Range(spawnArea.min.x, spawnArea.max.x),0, Random.Range(spawnArea.min.z, spawnArea.max.z));

        // Instantiate the zombie at the spawn position
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, enemiesParent);

        // reset enemy health
        newEnemy.GetComponent<EnemyController>().enemyData.ResetHealth();

    }

    public void SpawnAmmo(int ammoIndex){
        // Get a random position within the spawn area
        Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.min.x, spawnArea.max.x),
                spawnAreaHeight,
                Random.Range(spawnArea.min.z, spawnArea.max.z)
            );
        
        // Instantiate the ammo crate at the random position
        Instantiate(ammoCrates[ammoIndex], randomPosition, Quaternion.identity);

    }

    public void SpawnPowerup(int powerupIndex){
        // Get a random position within the spawn area
        Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.min.x, spawnArea.max.x),
                spawnAreaHeight,
                Random.Range(spawnArea.min.z, spawnArea.max.z)
            );
        
        // Instantiate the powerup at the random position
        Instantiate(powerups[powerupIndex], randomPosition, Quaternion.identity);

    }
}
