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
            text.text = $" {SaveData.Instance.HP}\n" +
                        $" {SaveData.Instance.resources}\n" +
                        $" {SaveData.Instance.energy}";
        }
    }
}
