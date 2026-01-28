using System;
using TMPro;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Identidad")]
    public bool isPlayerControlled = false;

    [Header("Configuración de Cuadrícula")]
    public float spacing = 1.1f;
    public int gridX, gridY;
    public int movesLeft = 10;

    private const int minBound = 0;
    private const int maxBound = 8;
    public TextMeshProUGUI uiText;

    void Start()
    {
        if (isPlayerControlled && uiText == null)
        {
            GameObject txt = GameObject.Find("MovementText");
            if (txt) uiText = txt.GetComponent<TextMeshProUGUI>();
        }
        SnapToGrid();
        UpdateUI();
    }

    void Update()
    {
        if (TurnManager.Instance.turnoActual != Turno.Jugador) return;
        if (!isPlayerControlled) return;
        if (movesLeft <= 0) return;

        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(0, -1);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
    }

    public bool TryMove(int dx, int dy)
    {
        if (movesLeft <= 0) return false;

        int tx = gridX + dx;
        int ty = gridY + dy;

        if (tx < minBound || tx > maxBound || ty < minBound || ty > maxBound) return false;

        if (!IsCellOccupied(tx, ty))
        {
            gridX = tx;
            gridY = ty;
            ConsumirMovimientos(1);
            SnapToGrid();
            return true;
        }

        return false;
    }

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

    public void SnapToGrid() => transform.position = new Vector3(gridX * spacing, gridY * spacing, 0);

    public bool IsCellOccupied(int gx, int gy)
    {
        Vector3 target = new Vector3(gx * spacing, gy * spacing, 0);

        foreach (var e in UnityEngine.Object.FindObjectsByType<ShipMovement>(FindObjectsSortMode.None))
        {
            if (e.gameObject == this.gameObject) continue;
            if (Vector2.Distance(new Vector2(e.transform.position.x, e.transform.position.y),
                                new Vector2(target.x, target.y)) < 0.1f) return true;
        }

        foreach (var o in UnityEngine.Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None))
        {
            if (Vector2.Distance(new Vector2(o.transform.position.x, o.transform.position.y),
                                new Vector2(target.x, target.y)) < 0.1f) return true;
        }

        return false;
    }

    public void UpdateUI()
    {
        if (isPlayerControlled && uiText != null)
            uiText.text = "Movimientos: " + movesLeft;
    }

    public void ConsumirMovimientos(int c)
    {
        movesLeft -= c;
        UpdateUI();
    }
}