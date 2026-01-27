using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Vector3 target;
    public bool isMoving = false;
    public int movements = 0;
    public int maxMovements = 3;

    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        target = transform.position;
    }

    public void MoveTo(Vector2 position)
    {
        if (!isMoving && movements < maxMovements)
        {
            target = position;
            isMoving = true;
            if (myCollider != null) myCollider.enabled = false;
        }
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector2.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.001f)
        {
            transform.position = target;
            isMoving = false;
            movements++;
            if (myCollider != null) myCollider.enabled = true;
        }
    }
}
