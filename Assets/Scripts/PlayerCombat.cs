using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerMovement playerMovement;
    
  
    [SerializeField] private Collider2D attackRange;
    [SerializeField] private string[] attacks = {"Cuerpo", "Distancia"};

    public int numAttacks = 1;

    void Awake()
    { 
        
        playerMovement = GetComponent<PlayerMovement>();
        
        
        if (attackRange == null) attackRange = GetComponent<BoxCollider2D>();
    }

    public void Atacar(GameObject enemigo)
    {
        if(attacks[0] == "Cuerpo")
        {
            Debug.Log("Atacando cuerpo a cuerpo a: " + enemigo.name);
            
        }
    }

    void Update()
    {
        
    }
}