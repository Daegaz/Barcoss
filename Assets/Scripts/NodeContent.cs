using UnityEngine;
using TMPro; // Required for TextMeshPro
public class NodeContent : MonoBehaviour
{
    // 1. Define what a "Headline" looks like now
    [System.Serializable]
    public class HeadlineOption
    {
        public string text; // The text the player sees (e.g. "Found an Apple")

        [Header("Active Stats")]
        public bool useHP;        // Check to include HP change
        public bool useResources; // Check to include Resource change
        public bool useEnergy;    // Check to include Energy change
    }
    
    // 2. Define the Categories (Positive/Negative ranges)
    [System.Serializable]
    public class Category
    {
        public string name; // "Positive", "Negative"

        [Header("Visuals & Behavior")]
        public Color textColor = Color.white; // Set Green for Positive, Red for Negative
        public bool isNeutral = false;        // If TRUE, button says "Continue" and stats are 0

        [Header("Configuration")]
        public int minRange;
        public int maxRange;

        [Header("Content")]
        public HeadlineOption[] headlines; // List of smart headlines
    }


    [Header("Setup")]
    public Category[] categories; // Set size to 3 (Positive, Neutral, Negative)

    [Header("UI References")]
    public TMP_Text headlineText;
    public TMP_Text buttonText;
    
    // --- PUBLIC DATA FOR OTHER SCRIPTS --- NodeContent.generatedValues[i]
    // Index 0 = HP
    // Index 1 = Resources
    // Index 2 = Energy
    [HideInInspector]
    public int[] generatedValues;

    public void Randomize()
    {
        if (categories.Length == 0) return;
        generatedValues = new int[3]; // Initialize array size

        // 1. Pick a Category (Positive, Negative, or Neutral)
        Category selectedCategory = categories[Random.Range(0, categories.Length)];

        // 2. Apply the Category Color to the button text
        buttonText.color = selectedCategory.textColor;

        // 3. Pick a Headline
        if (selectedCategory.headlines.Length > 0)
        {
            HeadlineOption selectedHeadline = selectedCategory.headlines[Random.Range(0, selectedCategory.headlines.Length)];
            headlineText.text = selectedHeadline.text;

            // 4. Determine Button Content
            if (selectedCategory.isNeutral)
            {
                // -- NEUTRAL BEHAVIOR --
                buttonText.text = "Continue";

                // Ensure values are zero so no stats change
                generatedValues[0] = 0;
                generatedValues[1] = 0;
                generatedValues[2] = 0;
            }
            else
            {
                // -- STANDARD BEHAVIOR (Positive/Negative) --
                CalculateStats(selectedCategory, selectedHeadline);
            }
        }
    }

    void CalculateStats(Category category, HeadlineOption headline)
    {
        string buttonString = "";

        int hpVal = 0;
        int resVal = 0;
        int enVal = 0;

        // HP
        if (headline.useHP)
        {
            hpVal = Random.Range(category.minRange, category.maxRange + 1);
            buttonString += FormatStat("HP", hpVal) + " ";
        }

        // Resources
        if (headline.useResources)
        {
            resVal = Random.Range(category.minRange, category.maxRange + 1);
            buttonString += FormatStat("Res", resVal) + " ";
        }

        // Energy
        if (headline.useEnergy)
        {
            enVal = Random.Range(category.minRange, category.maxRange + 1);
            buttonString += FormatStat("En", enVal) + " ";
        }

        // Save to public array
        generatedValues[0] = hpVal;
        generatedValues[1] = resVal;
        generatedValues[2] = enVal;

        buttonText.text = buttonString.Trim();
    }

    // Helper function to make the text look nice (e.g., "HP +5")
    string FormatStat(string label, int value)
    {
        string sign = (value > 0) ? "+" : "";
        return $"{label} {sign}{value}";
    }
}
