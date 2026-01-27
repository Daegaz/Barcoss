using UnityEngine;
using TMPro;

public class ShipMovement : MonoBehaviour
{
    public bool isPlayerControlled = false;
    public float spacing = 1.0f; // Asegúrate que sea 1.0f para que estén pegadas
    public int gridX, gridY;
    public int movesLeft = 10;
    public TextMeshProUGUI uiText;

    private const int minBound = 0;
    private const int maxBound = 8;

    void Start()
    {
        SnapToGrid();
        UpdateUI();
    }

    void Update()
    {
        if (TurnManager.Instance.turnoActual != Turno.Jugador || !isPlayerControlled || movesLeft <= 0) return;

        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(0, -1);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
    }

    void TryMove(int dx, int dy)
    {
        int tx = gridX + dx;
        int ty = gridY + dy;

        if (tx >= minBound && tx <= maxBound && ty >= minBound && ty <= maxBound)
        {
            if (!IsCellOccupied(tx, ty))
            {
                gridX = tx;
                gridY = ty;
                movesLeft--;
                SnapToGrid();
                UpdateUI();
            }
        }
    }

    // AHORA ES PÚBLICO para que el TurnManager lo use
    public bool IsCellOccupied(int gx, int gy)
    {
        Vector3 target = new Vector3(gx * spacing, gy * spacing, 0);
        return GridManager.IsCellOccupied(target);
    }

    public void SnapToGrid() => transform.position = new Vector3(gridX * spacing, gridY * spacing, 0);
    public void UpdateUI() { if (isPlayerControlled && uiText) uiText.text = "Movimientos: " + movesLeft; }
    public void ConsumirMovimientos(int c) { movesLeft -= c; UpdateUI(); }

    public void Retroceder(int dx, int dy, int fuerza)
    {
        for (int i = 0; i < fuerza; i++)
        {
            int nx = gridX + dx;
            int ny = gridY + dy;
            if (nx >= minBound && nx <= maxBound && ny >= minBound && ny <= maxBound && !IsCellOccupied(nx, ny))
            {
                gridX = nx;
                gridY = ny;
                SnapToGrid();
            }
            else break;
        }
    }
}