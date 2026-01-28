using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject selection;
    private PlayerMovement playerMovement ;
    public bool selectedTile;
    
    void Start()
    {
        playerMovement = GameObject.Find("Triangle").GetComponent<PlayerMovement>();
    }
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public void  OnMouseEnter()
    {
        if (selection.activeSelf && !IsEnemyAbove())
        {
            _highlight.SetActive(true); 
             selectedTile = true;
        }
     

    }
    public void OnMouseExit()
    {
        _highlight.SetActive(false);
        selectedTile = false;
    }
    void OnMouseDown()
   {
    
    int enemyLayerMask = LayerMask.GetMask("Enemy");
    
    
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, enemyLayerMask);

    
    if (hit.collider != null) 
    {
        return; 
    }

    
    if (selection.activeSelf && playerMovement != null)
    {
        playerMovement.MoveTo(transform.position);
    }
   }
bool IsEnemyAbove() {
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("Enemy"));
    return hit.collider != null;
}

}
