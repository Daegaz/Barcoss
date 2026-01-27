using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (player != null)
            transform.position = player.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("EnemyTrigger")) return;

        EnemySelection enemy = collision.GetComponent<EnemySelection>();
        if (enemy != null)
            enemy.selection.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("EnemyTrigger")) return;

        EnemySelection enemy = collision.GetComponent<EnemySelection>();
        if (enemy != null)
        {
            enemy.selection.SetActive(false);
            enemy._highlight.SetActive(false);
        }
    }
}