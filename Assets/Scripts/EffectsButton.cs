using UnityEngine;

public class EffectsButton : MonoBehaviour
{
    void OnEnable()
    {
        NodeManager.LockNodes();
    }
    void OnMouseDown()
    {
        Destroy(gameObject);
        NodeManager.UnlockNodes();
        
    }
}
