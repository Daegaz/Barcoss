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
        if (selection.activeSelf)
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
        if (selection.activeSelf)
        {
            playerMovement.MoveTo(transform.position);
        }
        

    }


}
