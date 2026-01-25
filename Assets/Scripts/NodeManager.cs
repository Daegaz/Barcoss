using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Transform shipSprite;
    public float moveSpeed = 5f;
    public GameObject textToActivate;
    public float yOffset = -0.3f;
    
    private static Transform currentTarget;
    private static GameObject currentText;

    void OnMouseDown()
    {
        if (currentText != null && currentText != textToActivate)
        {
            currentText.SetActive(false);
        }
        currentTarget = transform;
        currentText = textToActivate;
        currentText.SetActive(true);
    }

    void Update()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = currentTarget.position + new Vector3(0f, yOffset, 0f);

        shipSprite.position = Vector3.MoveTowards(
            shipSprite.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}
