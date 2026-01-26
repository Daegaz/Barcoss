using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject selection;
    
    private PlayerCombat playerCombat;

    void Start()
    {
        GameObject player = GameObject.Find("Triangle");
        if (player != null) playerCombat = player.GetComponent<PlayerCombat>();
        
        _highlight.SetActive(false);
        selection.SetActive(false);
    }

    
    public void OnMouseEnter()
    {
        if (selection.activeSelf) 
        {
            _highlight.SetActive(true); 
        }
    }

    public void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    
    void OnMouseDown()
    {
        if (selection.activeSelf && playerCombat != null)
        {
            Debug.Log("Atacando a: " + gameObject.name);
            
        }
    }
}