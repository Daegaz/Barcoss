using UnityEngine;

public class ShipCombat : MonoBehaviour
{
    [Header("Configuración General")]
    public float spacing = 1.1f;
    private ShipMovement movementScript;

    [Header("Arrow Shower (K)")]
    public float arrowRange = 2.3f;
    public int arrowCost = 2;

    [Header("Ramming (J)")]
    public int rammingCost = 1;
    public int fuerzaRetroceso = 2; // Puedes cambiar esto en el Inspector

    void Start()
    {
        movementScript = GetComponent<ShipMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) EjecutarRamming();
        if (Input.GetKeyDown(KeyCode.K)) EjecutarArrowShower();
    }

    void EjecutarArrowShower()
    {
        if (movementScript.movesLeft < arrowCost) return;

        GridEntity[] entities = Object.FindObjectsByType<GridEntity>(FindObjectsSortMode.None);
        bool hitTarget = false;

        foreach (GridEntity entity in entities)
        {
            float distance = Vector3.Distance(transform.position, entity.transform.position);
            if (distance > 0.1f && distance <= arrowRange)
            {
                ShipStats targetStats = entity.GetComponent<ShipStats>();
                if (targetStats != null)
                {
                    float danio = Random.Range(10f, 25f);
                    targetStats.RecibirDanio(0, danio);
                    hitTarget = true;
                }
            }
        }
        if (hitTarget) movementScript.ConsumirMovimientos(arrowCost);
    }

    void EjecutarRamming()
    {
        if (movementScript.movesLeft < rammingCost) return;

        GameObject enemyObj = GameObject.Find("ES");
        if (enemyObj != null)
        {
            float distanciaCeldas = Vector3.Distance(transform.position, enemyObj.transform.position) / spacing;

            if (distanciaCeldas > 0.1f && distanciaCeldas < 1.5f)
            {
                ShipStats targetStats = enemyObj.GetComponent<ShipStats>();
                if (targetStats != null)
                {
                    // 1. Daño
                    float danio = Random.Range(10f, 26f);
                    targetStats.RecibirDanio(danio, 0);
                    Debug.Log("¡RAMMING! Retrocediendo...");

                    // 2. Calcular dirección de retroceso
                    // Si el enemigo está en X=2 y yo en X=1, la diff es 1. Retroceso es -1.
                    int diffX = Mathf.RoundToInt((enemyObj.transform.position.x - transform.position.x) / spacing);
                    int diffY = Mathf.RoundToInt((enemyObj.transform.position.y - transform.position.y) / spacing);

                    // 3. Aplicar retroceso (invirtiendo la dirección de la colisión)
                    movementScript.Retroceder(-diffX, -diffY, fuerzaRetroceso);

                    movementScript.ConsumirMovimientos(rammingCost);
                }
            }
        }
    }
}