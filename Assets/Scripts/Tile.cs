using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject selection;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Triangle").GetComponent<PlayerMovement>();
        _highlight.SetActive(false);
        selection.SetActive(false);
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Tile"));
        bool isThisTileHovered = hit == GetComponent<Collider2D>();

        _highlight.SetActive(isThisTileHovered && selection.activeSelf);

        if (isThisTileHovered && selection.activeSelf && Input.GetMouseButtonDown(0))
        {
            playerMovement.MoveTo(transform.position);
        }
    }
}