using System.Collections.Generic;
using UnityEngine;

public class DestroyTracker : MonoBehaviour
{
    public static DestroyTracker Instance;

    // Tracks all objects that should be destroyed permanently
    private HashSet<string> destroyedObjects = new HashSet<string>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this from any object when it is permanently destroyed
    public void MarkDestroyed(string objectId)
    {
        if (!destroyedObjects.Contains(objectId))
        {
            destroyedObjects.Add(objectId);
            Debug.Log($"Object marked destroyed: {objectId}");
        }
    }

    // Call this from any object on Start() to see if it should be destroyed
    public bool IsDestroyed(string objectId)
    {
        return destroyedObjects.Contains(objectId);
    }
}
