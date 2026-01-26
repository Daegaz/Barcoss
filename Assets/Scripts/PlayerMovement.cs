using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Vector3 target;
    public bool isMoving = false;
    public int movements = 0;
    public int maxMovements = 3;
    
    private Tile currentTile; 
    private Collider2D myCollider; 

    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        target = transform.position;
    }

    public void MoveTo(Vector2 position)
    {
        if (!isMoving && movements < maxMovements) 
        {
            if (currentTile != null) currentTile.selection.SetActive(false);
            target = position;
            isMoving = true;
            if (myCollider != null) myCollider.enabled = false;
        }
    }

    void Update()
    {
        if (isMoving)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Activa la casilla actual
        Tile tile = collision.GetComponent<Tile>();
        if (tile != null)
        {
            currentTile = tile;
            currentTile.selection.SetActive(true);
        }

        // Activa al enemigo si estamos sobre él o cerca
        EnemySelection enemy = collision.GetComponent<EnemySelection>();
        if (enemy != null)
        {
            enemy.selection.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile != null)
        {
            tile.selection.SetActive(false);
            if (currentTile == tile) currentTile = null;
        }

        EnemySelection enemy = collision.GetComponent<EnemySelection>();
        if (enemy != null)
        {
            enemy.selection.SetActive(false);
        }
    }
}
