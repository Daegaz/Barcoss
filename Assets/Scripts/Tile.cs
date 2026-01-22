using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
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
        
        _highlight.SetActive(true); 
        selectedTile = true;

    }
    public void OnMouseExit()
    {
        _highlight.SetActive(false);
        selectedTile = false;
    }
    void OnMouseDown()
    {
        playerMovement.MoveTo(transform.position);
    }


}
