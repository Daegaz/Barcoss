using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject selection;

    private PlayerCombat playerCombat;

    void Start()
    {
        GameObject player = GameObject.Find("Triangle");
        if (player != null)
            playerCombat = player.GetComponent<PlayerCombat>();

        _highlight.SetActive(false);
        selection.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Enemy"));
        bool isThisEnemyHovered = hit == GetComponent<Collider2D>();

        _highlight.SetActive(isThisEnemyHovered && selection.activeSelf);

        if (isThisEnemyHovered && selection.activeSelf && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Atacando a: " + gameObject.name);
            // Llama aquí a playerCombat.AttackEnemy(this) si quieres
        }
    }
}