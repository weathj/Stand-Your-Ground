using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundController : MonoBehaviour
{

    private GameObject[] groundTiles;
    public List<GameObject> groundTilePrefabs;

    [SerializeField]
    private float groundTileCount = 0f;
    private float tilesNeeded = 8f;
    private PlayerController player;
    [SerializeField]
    private float groundTileOffset = 10f;
    [SerializeField]
    private float dropoffDistance = 100f;
    private float lastTileDistance;
    [SerializeField]
    private float lastTileOffset;
    [SerializeField]
    private Transform roadParent;
    [SerializeField]
    private TMP_Text tileCountText;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame

    void Update()
    {
        // Get all the ground tiles
        groundTiles = GameObject.FindGameObjectsWithTag("Ground");

        groundTileCount = groundTiles.Length;

        // If there are less than 8 ground tiles, spawn more
        if (groundTileCount < tilesNeeded)
        {
            SpawnGroundTile();
        }

        lastTileDistance = groundTiles[groundTiles.Length - 1].transform.position.z - player.distance;
        if (lastTileDistance < lastTileOffset){
            SpawnGroundTile();
        }

        // for loop to find any ground tiles that are too far away
        for (int i = 0; i < groundTiles.Length; i++)
        {
            if (groundTiles[i].transform.position.z < player.distance - dropoffDistance)
            {
                Destroy(GameObject.Find(groundTiles[i].name));
            }
        }


        tileCountText.text = groundTileCount.ToString();
        
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
