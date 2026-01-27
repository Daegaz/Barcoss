using UnityEngine;
using System;

public class ShipCombat : MonoBehaviour
{
    public float spacing = 1.1f;
    private ShipMovement movement;
    private System.Random rng = new System.Random();

    public int arrowRange = 6;
    public int arrowCost = 2;

    void Start() => movement = GetComponent<ShipMovement>();

    void Update()
    {
        // BLOQUEO DE TURNO
        if (TurnManager.Instance.turnoActual != Turno.Jugador) return;

        if (Input.GetKeyDown(KeyCode.J)) EjecutarRamming();
        if (Input.GetKeyDown(KeyCode.K)) EjecutarArrowShower();
    }

    void EjecutarArrowShower()
    {
        if (movement.movesLeft < arrowCost) return;
        GameObject enemy = GameObject.Find("ES");
        if (enemy == null) return;

        float diffX = Mathf.Abs(enemy.transform.position.x - transform.position.x);
        float diffY = Mathf.Abs(enemy.transform.position.y - transform.position.y);

        bool alineadoHorizontal = diffY < 0.2f;
        bool alineadoVertical = diffX < 0.2f;
        float distanciaCeldas = (diffX + diffY) / spacing;

        if ((alineadoHorizontal || alineadoVertical) && distanciaCeldas <= (arrowRange + 0.1f))
        {
            ShipStats stats = enemy.GetComponent<ShipStats>();
            if (stats != null)
            {
                float danio = (float)rng.Next(10, 21);
                stats.RecibirDanio(danio);
                movement.ConsumirMovimientos(arrowCost);
            }
        }
    }

    void EjecutarRamming()
    {
        if (movement.movesLeft < 1) return;
        GameObject enemy = GameObject.Find("ES");
        if (enemy == null) return;

        float dX = enemy.transform.position.x - transform.position.x;
        float dY = enemy.transform.position.y - transform.position.y;
        float dist = Vector3.Distance(transform.position, enemy.transform.position);

        if (dist > 0.5f && dist < (spacing + 0.2f))
        {
            ShipStats stats = enemy.GetComponent<ShipStats>();
            if (stats != null)
            {
                stats.RecibirDanio((float)rng.Next(15, 26));
                int dirX = Mathf.RoundToInt(dX / spacing);
                int dirY = Mathf.RoundToInt(dY / spacing);
                movement.Retroceder(-dirX, -dirY, 2);
                movement.ConsumirMovimientos(1);
            }
        }
    }
}