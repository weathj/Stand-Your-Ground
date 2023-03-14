using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVLogger : MonoBehaviour
{
    
    // Singleton instance

    public static CSVLogger Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
