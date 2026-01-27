using UnityEngine;
using TMPro;
public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    // Data to keep track of
    public int HP = 100;
    public int resources = 0;
    public int energy = 10;

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

    //TO WRITE THE STATS IN THE TEXT
    public TextMeshProUGUI text; // Assign your child Text in the inspector

    void Update()
    {
        if (SaveData.Instance != null && text != null)
        {
            text.text = $"SHIP HP: {SaveData.Instance.HP}\n" +
                        $"Resources: {SaveData.Instance.resources}\n" +
                        $"Energy: {SaveData.Instance.energy}";
        }
    }

    
}
