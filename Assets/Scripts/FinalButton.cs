using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalButton : MonoBehaviour
{
    public Transform shipSprite;
    
    [Header("Range Settings")]
    public float interactionRange = 5f; // The max distance allowed

    void OnMouseDown() { 
        float distance = Vector2.Distance(shipSprite.position, transform.position);
        if (distance > interactionRange)
        {
            return;
        
        }
        else
        {
            SaveData.Instance.startPosition = new Vector3(1.352f, 2.9749999f, 0f);
            
            SceneManager.LoadScene("Exploration 2");
        }
    }
    void OnDrawGizmos()
    {
        // Sets the color of the circle
        Gizmos.color = Color.cyan;

        // Draws a wireframe circle around the node
        // It uses 'transform.position' as the center and 'interactionRange' as the size
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
