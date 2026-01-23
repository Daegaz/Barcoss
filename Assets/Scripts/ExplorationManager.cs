using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    public static ExplorationManager Instance;
    private bool active;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableExploration(bool enabled)
    {
        active = enabled;
        // enable/disable UI, player movement, etc
    }

    private void Update()
    {
        if (!active) return;

        // Exploration logic
        // Example: detect enemy ? enter combat
    }

    public void TriggerCombat()
    {
        GameController.Instance.EnterCombat();
    }
}
