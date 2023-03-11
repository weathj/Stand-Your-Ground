using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{

    private GameObject[] groundTiles;
    public List<GameObject> groundTilePrefabs;

    [SerializeField]
    private float groundTileCount = 0f;
    private float tilesNeeded = 8f;
    private GameObject player;
    [SerializeField]
    private float groundTileOffset = 10f;
    [SerializeField]
    private Transform roadParent;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame

    void Update()
    {
        // Get all the ground tiles
        groundTiles = GameObject.FindGameObjectsWithTag("Ground");

        // If there are less than 8 ground tiles, spawn more
        if (groundTiles.Length < tilesNeeded)
        {
            SpawnGroundTile();
        }

        // If there are more than 8 ground tiles, destroy the first one
        if (groundTiles.Length > tilesNeeded)
        {
            Destroy(groundTiles[0]);
        }
        
    }

    void SpawnGroundTile()
    {
        // Get a random ground tile
        int randomTile = Random.Range(0, groundTilePrefabs.Count);

        // Get the last ground tile
        GameObject lastGroundTile = groundTiles[groundTiles.Length - 1];

        // Get the snapback of the last ground tile
        Transform snapback = lastGroundTile.transform.Find("Snapback");

        // Spawn the ground tile
        GameObject groundTile = Instantiate(groundTilePrefabs[randomTile], snapback.position, Quaternion.identity, roadParent);

        // Set the parent of the ground tile to the ground controller
        groundTile.transform.parent = gameObject.transform;

        // Increment the ground tile count
        groundTileCount++;
    }

}
