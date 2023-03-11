using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float zbound = 20f;
    [SerializeField]
    private float xbound = 5f;
    [SerializeField]
    private float spawnHeight = -0.25f;
    public GameObject enemy;
    public float spawnTime = 5f;
    public float enemyCount = 0f;
    private bool enemyNeeded = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount < 10 && enemyNeeded)
        {
            enemyNeeded = false;
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        enemyCount++;
        yield return new WaitForSeconds(spawnTime);
        Vector3 spawnPosition = new Vector3(Random.Range(-xbound, xbound), 0.1f, Random.Range(10, zbound));
        Instantiate(enemy, spawnPosition, Quaternion.identity);
        enemyNeeded = true;
    }
}
