//using System;
//using TMPro;
//using UnityEngine;

//public class ShipMovement : MonoBehaviour
//{
//    [Header("Identidad")]
//    public bool isPlayerControlled = false;

//    [Header("Configuración de Cuadrícula")]
//    public float spacing = 1.1f;
//    public int gridX, gridY;
//    public int movesLeft = 10;

//    private const int minBound = 0;
//    private const int maxBound = 8;
//    public TextMeshProUGUI uiText;

//    void Start()
//    {
//        if (isPlayerControlled && uiText == null)
//        {
//            UnityEngine.GameObject txt = UnityEngine.GameObject.Find("MovementText");
//            if (txt) uiText = txt.GetComponent<TextMeshProUGUI>();
//        }
//        SnapToGrid();
//        UpdateUI();
//    }

//    void Update()
//    {
//        if (TurnManager.Instance.turnoActual != TurnManager.Turno.Jugador) return;
//        if (!isPlayerControlled || movesLeft <= 0) return;

//        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.W)) TryMove(0, 1);
//        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.S)) TryMove(0, -1);
//        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.A)) TryMove(-1, 0);
//        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.D)) TryMove(1, 0);
//    }

//    public int ObtenerCosteAccion(int costeBase)
//    {
//        SlowCell[] ralentizadores = UnityEngine.Object.FindObjectsByType<SlowCell>(UnityEngine.FindObjectsSortMode.None);
//        foreach (SlowCell sc in ralentizadores)
//        {
//            if (UnityEngine.Vector2.Distance(transform.position, sc.transform.position) < 0.1f)
//            {
//                return costeBase * sc.multiplicadorCoste;
//            }
//        }
//        return costeBase;
//    }

//    public bool TryMove(int dx, int dy)
//    {
//        int costeFinal = ObtenerCosteAccion(1);

//        // --- REGLA DE PIEDAD ---
//        // Si el coste es mayor a 1 (estás en lodo) pero solo te queda 1 movimiento,
//        // te dejamos moverte para que no te quedes bugeado.
//        if (movesLeft < costeFinal && movesLeft == 1)
//        {
//            costeFinal = 1;
//        }

//        if (movesLeft < costeFinal) return false;

//        int tx = gridX + dx;
//        int ty = gridY + dy;

//        if (tx < minBound || tx > maxBound || ty < minBound || ty > maxBound) return false;

//        if (!IsCellOccupied(tx, ty))
//        {
//            gridX = tx;
//            gridY = ty;
//            ConsumirMovimientos(costeFinal);
//            SnapToGrid();
//            return true;
//        }
//        return false;
//    }

//    public void Retroceder(int dx, int dy, int fuerza)
//    {
//        for (int i = 0; i < fuerza; i++)
//        {
//            int nx = gridX + dx;
//            int ny = gridY + dy;
//            if (nx >= minBound && nx <= maxBound && ny >= minBound && ny <= maxBound && !IsCellOccupied(nx, ny))
//            {
//                gridX = nx;
//                gridY = ny;
//                SnapToGrid();
//            }
//            else break;
//        }
//    }

//    public void SnapToGrid()
//    {
//        transform.position = new UnityEngine.Vector3(gridX * spacing, gridY * spacing, 0);
//        CheckForHazards();
//    }

//    private void CheckForHazards()
//    {
//        HazardCell[] hazards = UnityEngine.Object.FindObjectsByType<HazardCell>(UnityEngine.FindObjectsSortMode.None);
//        foreach (HazardCell h in hazards)
//        {
//            if (UnityEngine.Vector2.Distance(transform.position, h.transform.position) < 0.1f)
//            {
//                h.AplicarDanio(this.gameObject);
//                break;
//            }
//        }
//    }

//    public bool IsCellOccupied(int gx, int gy)
//    {
//        UnityEngine.Vector3 target = new UnityEngine.Vector3(gx * spacing, gy * spacing, 0);
//        ShipMovement[] allShips = UnityEngine.Object.FindObjectsByType<ShipMovement>(UnityEngine.FindObjectsSortMode.None);
//        foreach (var e in allShips)
//        {
//            if (e.gameObject == this.gameObject) continue;
//            if (UnityEngine.Vector2.Distance(e.transform.position, target) < 0.1f) return true;
//        }
//        Obstacle[] allObstacles = UnityEngine.Object.FindObjectsByType<Obstacle>(UnityEngine.FindObjectsSortMode.None);
//        foreach (var o in allObstacles)
//        {
//            if (UnityEngine.Vector2.Distance(o.transform.position, target) < 0.1f) return true;
//        }
//        return false;
//    }

//    public void UpdateUI() { if (isPlayerControlled && uiText != null) uiText.text = "Accions restants: " + movesLeft; }
//    public void ConsumirMovimientos(int c) { movesLeft -= c; UpdateUI(); }
//}

using System.Collections; // Necesario para Corrutinas
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

    [Header("Animación")]
    public float moveSpeed = 8f; // Velocidad del deslizamiento
    private bool isMoving = false; // Bloquea el input mientras se desliza

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
        // Posición inicial instantánea
        transform.position = new Vector3(gridX * spacing, gridY * spacing, 0);
        UpdateUI();
    }

    void Update()
    {
        if (TurnManager.Instance.turnoActual != TurnManager.Turno.Jugador) return;
        if (!isPlayerControlled || movesLeft <= 0 || isMoving) return; // No mover si ya se está deslizando

        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(0, -1);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
    }

    public bool TryMove(int dx, int dy)
    {
        int costeFinal = ObtenerCosteAccion(1);

        if (movesLeft < costeFinal && movesLeft == 1) costeFinal = 1;
        if (movesLeft < costeFinal) return false;

        int tx = gridX + dx;
        int ty = gridY + dy;

        if (tx < minBound || tx > maxBound || ty < minBound || ty > maxBound) return false;

        if (!IsCellOccupied(tx, ty))
        {
            gridX = tx;
            gridY = ty;
            ConsumirMovimientos(costeFinal);

            // CAMBIO: En lugar de SnapToGrid, iniciamos el deslizamiento
            Vector3 targetPos = new Vector3(gridX * spacing, gridY * spacing, 0);
            StartCoroutine(SmoothMove(targetPos));
            return true;
        }
        return false;
    }

    // --- NUEVA CORRUTINA PARA DESLIZAR ---
    IEnumerator SmoothMove(Vector3 target)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null; // Espera al siguiente frame
        }

        transform.position = target; // Asegurar posición exacta
        CheckForHazards(); // Comprobar peligros al llegar
        isMoving = false;
    }

    // Modificamos Retroceder para que también sea suave
    public void Retroceder(int dx, int dy, int fuerza)
    {
        int tx = gridX;
        int ty = gridY;

        for (int i = 0; i < fuerza; i++)
        {
            int nx = tx + dx;
            int ny = ty + dy;
            if (nx >= minBound && nx <= maxBound && ny >= minBound && ny <= maxBound && !IsCellOccupied(nx, ny))
            {
                tx = nx;
                ty = ny;
            }
            else break;
        }
        gridX = tx;
        gridY = ty;
        StartCoroutine(SmoothMove(new Vector3(gridX * spacing, gridY * spacing, 0)));
    }

    // Mantengo SnapToGrid por si otros scripts lo llaman (ej: resetear nivel)
    public void SnapToGrid()
    {
        transform.position = new Vector3(gridX * spacing, gridY * spacing, 0);
        CheckForHazards();
    }

    // --- EL RESTO DE TUS MÉTODOS (ObtenerCosteAccion, IsCellOccupied, etc.) SE QUEDAN IGUAL ---
    public int ObtenerCosteAccion(int costeBase)
    {
        SlowCell[] ralentizadores = UnityEngine.Object.FindObjectsByType<SlowCell>(UnityEngine.FindObjectsSortMode.None);
        foreach (SlowCell sc in ralentizadores)
        {
            if (UnityEngine.Vector2.Distance(transform.position, sc.transform.position) < 0.1f)
                return costeBase * sc.multiplicadorCoste;
        }
        return costeBase;
    }

    private void CheckForHazards()
    {
        HazardCell[] hazards = UnityEngine.Object.FindObjectsByType<HazardCell>(UnityEngine.FindObjectsSortMode.None);
        foreach (HazardCell h in hazards)
        {
            if (UnityEngine.Vector2.Distance(transform.position, h.transform.position) < 0.1f)
            {
                h.AplicarDanio(this.gameObject);
                break;
            }
        }
    }

    public bool IsCellOccupied(int gx, int gy)
    {
        Vector3 target = new Vector3(gx * spacing, gy * spacing, 0);
        ShipMovement[] allShips = FindObjectsByType<ShipMovement>(FindObjectsSortMode.None);
        foreach (var e in allShips)
        {
            if (e.gameObject == this.gameObject) continue;
            if (Vector2.Distance(e.transform.position, target) < 0.1f) return true;
        }
        Obstacle[] allObstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        foreach (var o in allObstacles)
        {
            if (Vector2.Distance(o.transform.position, target) < 0.1f) return true;
        }
        return false;
    }

    public void UpdateUI() { if (isPlayerControlled && uiText != null) uiText.text = "Accions restants: " + movesLeft; }
    public void ConsumirMovimientos(int c) { movesLeft -= c; UpdateUI(); }
}