using UnityEngine;
using TMPro;

public class ShipMovement : MonoBehaviour
{
    [Header("Configuración de Cuadrícula")]
    public float spacing = 1.1f;
    public int gridX = 1;
    public int gridY = 4;
    public int movesLeft = 10;

    [Header("Referencia UI")]
    public TextMeshProUGUI uiText;

    void Start()
    {
        if (uiText == null)
        {
            GameObject textObject = GameObject.Find("MovementText");
            if (textObject != null)
                uiText = textObject.GetComponent<TextMeshProUGUI>();
        }

        UpdatePosition();
        UpdateUI();
    }

    void Update()
    {
        if (movesLeft > 0)
        {
            if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
            if (Input.GetKeyDown(KeyCode.S)) TryMove(0, -1);
            if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
            if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
        }
    }

    // Nueva función de retroceso
    public void Retroceder(int dirX, int dirY, int fuerza)
    {
        for (int i = 0; i < fuerza; i++)
        {
            int nextX = gridX + dirX;
            int nextY = gridY + dirY;
            Vector3 targetWorldPos = new Vector3(nextX * spacing, nextY * spacing, 0);

            // Comprobar límites y colisiones
            if (nextX >= 0 && nextX < 10 && nextY >= 0 && nextY < 10)
            {
                if (!IsCellOccupied(targetWorldPos))
                {
                    gridX = nextX;
                    gridY = nextY;
                    UpdatePosition();
                }
                else { break; } // Chocó con algo, para el retroceso
            }
            else { break; } // Límite del mapa
        }
    }

    public bool ConsumirMovimientos(int cantidad)
    {
        if (movesLeft >= cantidad)
        {
            movesLeft -= cantidad;
            UpdateUI();
            return true;
        }
        return false;
    }

    void TryMove(int xDelta, int yDelta)
    {
        int targetX = gridX + xDelta;
        int targetY = gridY + yDelta;
        Vector3 targetWorldPos = new Vector3(targetX * spacing, targetY * spacing, 0);

        if (targetX >= 0 && targetX < 10 && targetY >= 0 && targetY < 10)
        {
            if (IsCellOccupied(targetWorldPos))
            {
                Debug.Log("Casilla ocupada.");
                return;
            }

            gridX = targetX;
            gridY = targetY;
            UpdatePosition();
            movesLeft--;
            UpdateUI();
        }
    }

    public void UpdatePosition() => transform.position = new Vector3(gridX * spacing, gridY * spacing, 0);

    public bool IsCellOccupied(Vector3 target)
    {
        GridEntity[] entities = Object.FindObjectsByType<GridEntity>(FindObjectsSortMode.None);
        foreach (GridEntity entity in entities)
        {
            if (Vector3.Distance(entity.transform.position, target) < 0.1f) return true;
        }
        return false;
    }

    void UpdateUI() { if (uiText != null) uiText.text = "Movimientos: " + movesLeft; }
}