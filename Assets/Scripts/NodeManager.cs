using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Transform shipSprite;
    public float moveSpeed = 5f;
    public GameObject textToActivate;
    public float yOffset = -0.3f;
    public GameObject EffectsButton;
    public GameObject VisitedText;
    public GameObject OutOfRangeText;

    [Header("Range Settings")]
    public float interactionRange = 5f; // The max distance allowed


    private static Transform currentTarget;
    private static GameObject currentText;
    private static bool isMoving = false;
    private static bool canClickNodes = true; // Starts unlocked
    void OnMouseDown()
    {
        if (!canClickNodes) return; // If it's false, the click is ignored

        // ... rest of your distance check and movement code ...

        if (isMoving) return; // Ignore click if already moving

        // ... your existing distance check and target setting ...
        
        isMoving = true; // Lock other nodes
        VisitedText.SetActive(false);
        float distance = Vector2.Distance(shipSprite.position, transform.position);
        if (distance > interactionRange)
        {
            VisitedText.SetActive(false);
            OutOfRangeText.SetActive(true);
            return;
        }
        if (currentText != null && currentText != textToActivate)
        {
            currentText.SetActive(false);
        }
        OutOfRangeText.SetActive(false);
        currentTarget = transform;
        currentText = textToActivate;
        
        SaveData.Instance.energy--;
        
        if (EffectsButton == null)
            VisitedText.SetActive(true);
    }

    void Update()
    {
        if (currentTarget == null) return;
        if (currentTarget != transform) return;
        Vector3 targetPosition = currentTarget.position + new Vector3(0f, yOffset, 0f);

        float dt = Mathf.Min(Time.deltaTime, 0.02f);

        shipSprite.position = Vector3.MoveTowards(
            shipSprite.position,
            targetPosition,
            moveSpeed * dt
        );
        if (shipSprite.position == targetPosition) 
        {
           
            isMoving = false;
            if (EffectsButton != null)
            {
                currentText.SetActive(true);
                EffectsButton.SetActive(true);
            }
            else
            {
                currentText.SetActive(false);
            }
            
        }
      


    }
    public static void UnlockNodes()
    {
        canClickNodes = true;
        Debug.Log("Nodes are now UNLOCKED");
    }

    public static void LockNodes()
    {
        canClickNodes = false;
        Debug.Log("Nodes are now LOCKED");
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
