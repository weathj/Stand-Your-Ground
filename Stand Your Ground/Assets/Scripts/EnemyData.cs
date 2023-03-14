using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string zName;
    public float health;
    public float speed;
    public float attack;
    public float spawnProbability;

    private float initialHealth;

    private void OnEnable()
    {
        initialHealth = health;
    }

    public void ResetHealth()
    {
        health = initialHealth;
    }

}
