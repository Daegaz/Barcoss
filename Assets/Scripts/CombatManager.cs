using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    private bool active;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableCombat(bool enabled)
    {
        active = enabled;

        if (enabled)
            StartCombat();
        else
            EndCombat();
    }

    private void StartCombat()
    {
        Debug.Log("Combat Started");
        // Spawn enemies, setup UI, etc
    }

    private void EndCombat()
    {
        Debug.Log("Combat Ended");
        // Cleanup combat stuff
    }

    private void Update()
    {
        if (!active) return;

        // Combat logic
    }

    public void DealDamageToPlayer(int damage)
    {
        GameController.Instance.ChangeHP(-damage);
    }

    public void ConsumeResource(int amount)
    {
        GameController.Instance.ChangeResources(-amount);
    }

    public void OnCombatWon()
    {
        GameController.Instance.ExitCombat();
    }
}