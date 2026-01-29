using UnityEngine;
using UnityEngine.SceneManagement;
public class EffectsButton : MonoBehaviour
{
    // --- PUBLIC DATA FOR OTHER SCRIPTS --- NodeContent.generatedValues[i]
    // Index 0 = HP
    // Index 1 = Resources
    // Index 2 = Energy
    [SerializeField] private string objectId;
    [SerializeField] private NodeContent nodeContent;
    [SerializeField] private LevelLoader LevelLoader;
    public GameObject shipSprite;
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
        if (nodeContent.isCombatCheck)
        {
            //Enter combat
            SaveData.Instance.startPosition= shipSprite.transform.position;
            SceneManager.LoadScene("Combat 2");
            
        }
        else 
        { 
            SaveData.Instance.TakeDamage(nodeContent.generatedValues[0]);
            SaveData.Instance.HP = Mathf.Max(0, SaveData.Instance.HP);
            SaveData.Instance.AddResources(nodeContent.generatedValues[1]);
            SaveData.Instance.resources = Mathf.Max(0, SaveData.Instance.resources);
            SaveData.Instance.AddEnergy(nodeContent.generatedValues[2]);
            SaveData.Instance.energy = Mathf.Max(0, SaveData.Instance.energy);
        }
    }
}
