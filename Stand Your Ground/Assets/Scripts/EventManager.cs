using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Example events
    public UnityEvent<float> distanceReachedEvent;
    public UnityEvent playerDiedEvent;
    public UnityEvent powerupCollectedEvent;

    // Singleton instance
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RaiseDistanceReachedEvent(float distance)
    {
        if (distanceReachedEvent != null)
        {
            distanceReachedEvent.Invoke(distance);
        }
    }

    // Other event raising methods here...
}

