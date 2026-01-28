using UnityEngine;

public class EffectsButton : MonoBehaviour
{
    // --- PUBLIC DATA FOR OTHER SCRIPTS --- NodeContent.generatedValues[i]
    // Index 0 = HP
    // Index 1 = Resources
    // Index 2 = Energy
    [SerializeField] private string objectId;

    [SerializeField] private SaveData saveData;
    [SerializeField] private NodeContent nodeContent;

    void OnEnable()
    {
        if (DestroyTracker.Instance != null && DestroyTracker.Instance.IsDestroyed(objectId))
        {
            Destroy(gameObject);
        }
        else
        {
            nodeContent.Randomize();
            NodeManager.LockNodes();
        }
    }
    void OnMouseDown()
    {
        if (DestroyTracker.Instance != null)
            DestroyTracker.Instance.MarkDestroyed(objectId);
        
        Destroy(gameObject);
        NodeManager.UnlockNodes();
        saveData.TakeDamage(nodeContent.generatedValues[0]);
        saveData.AddResources(nodeContent.generatedValues[1]);
        saveData.AddEnergy(nodeContent.generatedValues[2]);
    }
}
