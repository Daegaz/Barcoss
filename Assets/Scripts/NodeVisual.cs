using UnityEngine;
using UnityEngine.UI;

public class NodeVisual : MonoBehaviour
{
    public Image backgroundImage; // optional, assign in prefab
    public Color visitedColor = Color.gray;
    public Color unlockedColor = Color.green;
    public Color lockedColor = Color.red;

    // Set the visual state of the node
    public void SetState(bool visited, bool unlocked)
    {
        if (backgroundImage == null) return;

        if (visited)
            backgroundImage.color = visitedColor;
        else if (unlocked)
            backgroundImage.color = unlockedColor;
        else
            backgroundImage.color = lockedColor;
    }
}
