using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalButton2 : MonoBehaviour
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
            SceneManager.LoadScene("Victory");
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
