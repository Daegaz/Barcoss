using UnityEngine;
using TMPro;
public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    // Data to keep track of
    public int HP = 100;
    public int resources = 0;
    public int energy = 10;
    public Transform targetObject;
    [HideInInspector]
    public Vector3 startPosition;
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
        startPosition = targetObject.position;
    }

    // Optional: helper functions to modify values
    public void TakeDamage(int amount)
    {
            HP += amount;
        HP = Mathf.Max(HP, 0);
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }

    public void AddEnergy(int amount)
    {
        energy += amount;
    }
}
