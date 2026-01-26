using UnityEngine;
using TMPro;
public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    // Data to keep track of
    public int HP1 = 100;
    public int HP2 = 100;
    public int resources = 0;
    public int gold = 0;

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
    public void TakeDamage(int amount, int playerNumber)
    {
        if (playerNumber == 1)
            HP1 -= amount;
        else if (playerNumber == 2)
            HP2 -= amount;

        HP1 = Mathf.Max(HP1, 0);
        HP2 = Mathf.Max(HP2, 0);
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    //TO WRITE THE STATS IN THE TEXT
    public TextMeshProUGUI text; // Assign your child Text in the inspector

    void Update()
    {
        if (SaveData.Instance != null && text != null)
        {
            text.text = $"SHIP HP: {SaveData.Instance.HP1}\n" +
                        $"CREW HP: {SaveData.Instance.HP2}\n" +
                        $"Resources: {SaveData.Instance.resources}\n" +
                        $"Gold: {SaveData.Instance.gold}";
        }
    }
}
