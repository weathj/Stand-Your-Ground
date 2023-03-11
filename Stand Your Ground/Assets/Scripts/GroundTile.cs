using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
 
    public Transform snapfront;
    public Transform snapback;

    // Start is called before the first frame update
    void Start()
    {
        snapfront = transform.Find("Snapfront");
        snapback = transform.Find("Snapback");
    }

}
