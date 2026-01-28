using UnityEngine;

public class ShipCombat : MonoBehaviour
{
    public float spacing = 1.1f;
    private ShipMovement movement;

    public int arrowRange = 5;
    public int arrowCost = 2;

    void Start() => movement = GetComponent<ShipMovement>();

    void Update()
    {
        // CORRECCIÓN: Referencia al enum dentro de TurnManager
        if (TurnManager.Instance.turnoActual != TurnManager.Turno.Jugador) return;

        if (movement.isPlayerControlled)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.J)) EjecutarRamming();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.K)) EjecutarArrowShower();
        }
    }

    void EjecutarArrowShower()
    {
        int costeReal = movement.ObtenerCosteAccion(arrowCost);
        if (movement.movesLeft < costeReal) return;

        UnityEngine.GameObject enemy = UnityEngine.GameObject.Find("ES");
        if (enemy == null) return;

        float diffX = UnityEngine.Mathf.Abs(enemy.transform.position.x - transform.position.x);
        float diffY = UnityEngine.Mathf.Abs(enemy.transform.position.y - transform.position.y);

        bool alineadoHorizontal = diffY < 0.2f;
        bool alineadoVertical = diffX < 0.2f;
        float distanciaCeldas = (diffX + diffY) / spacing;

        if ((alineadoHorizontal || alineadoVertical) && distanciaCeldas <= (arrowRange + 0.1f))
        {
            ShipStats stats = enemy.GetComponent<ShipStats>();
            if (stats != null)
            {
                stats.RecibirDanio(UnityEngine.Random.Range(10f, 21f));
                movement.ConsumirMovimientos(costeReal);
            }
        }
    }

    void EjecutarRamming()
    {
        int costeReal = movement.ObtenerCosteAccion(1);
        if (movement.movesLeft < costeReal) return;

        UnityEngine.GameObject enemy = UnityEngine.GameObject.Find("ES");
        if (enemy == null) return;

        float dX = enemy.transform.position.x - transform.position.x;
        float dY = enemy.transform.position.y - transform.position.y;
        float dist = UnityEngine.Vector3.Distance(transform.position, enemy.transform.position);

        if (dist > 0.5f && dist < (spacing + 0.2f))
        {
            ShipStats stats = enemy.GetComponent<ShipStats>();
            if (stats != null)
            {
                stats.RecibirDanio(UnityEngine.Random.Range(10f, 26f));

                int dirX = UnityEngine.Mathf.RoundToInt(dX / spacing);
                int dirY = UnityEngine.Mathf.RoundToInt(dY / spacing);
                movement.Retroceder(-dirX, -dirY, 2);
                movement.ConsumirMovimientos(costeReal);
            }
        }
    }
}