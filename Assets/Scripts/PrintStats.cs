using UnityEngine;
using TMPro;
public class PrintStats : MonoBehaviour
{
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
