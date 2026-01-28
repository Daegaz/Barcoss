using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject allyPrefab;
    public GameObject enemyPrefab;
    public GameObject islandPrefab;
    public GameObject hazardPrefab;
    public GameObject slowPrefab;

    public int rows = 9;
    public int cols = 9;
    public float spacing = 1.1f;

    [Header("Configuración de Cantidades")]
    public int numeroIslas = 3;
    public int numeroZonasLentas = 3;
    public int numeroTrampas = 5;

    void Start() => GenerateGrid();

    void GenerateGrid()
    {
        // PASO 0: Crear el suelo visual (opcional si ya tienes fondo)
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
                UnityEngine.Object.Instantiate(cellPrefab, pos, Quaternion.identity, transform);
            }
        }

        // PASO 1: Spawn de Barcos (Prioridad absoluta en sus columnas)
        SpawnPlayerFixed();
        SpawnEnemyFixed();

        // PASO 2: Islas (4/8 -> 1x1, 3/8 -> 2x2, 1/8 -> 3x3)
        for (int i = 0; i < numeroIslas; i++)
        {
            int r = UnityEngine.Random.Range(1, 9);
            Vector2Int size = (r <= 4) ? new Vector2Int(1, 1) : (r <= 7) ? new Vector2Int(2, 2) : new Vector2Int(3, 3);
            TryPlaceShape(size, "Island");
        }

        // PASO 3: Ralentización (4/8 -> 1, 3/8 -> Linea 2, 1/8 -> Linea 3)
        for (int i = 0; i < numeroZonasLentas; i++)
        {
            int r = UnityEngine.Random.Range(1, 9);
            int length = (r <= 4) ? 1 : (r <= 7) ? 2 : 3;
            Vector2Int size = (UnityEngine.Random.value > 0.5f) ? new Vector2Int(length, 1) : new Vector2Int(1, length);
            TryPlaceShape(size, "Slow");
        }

        // PASO 4: Daño (Aleatorias 1x1)
        for (int i = 0; i < numeroTrampas; i++)
        {
            TryPlaceShape(new Vector2Int(1, 1), "Hazard");
        }
    }

    void TryPlaceShape(Vector2Int size, string type)
    {
        int intentos = 0;
        while (intentos < 50)
        {
            // Restringimos a la zona central (columnas 1 a 7 para dejar espacio a barcos)
            int rx = UnityEngine.Random.Range(1, rows - size.x);
            int ry = UnityEngine.Random.Range(1, cols - size.y);

            if (CanPlace(rx, ry, size.x, size.y))
            {
                PlaceEntity(rx, ry, size.x, size.y, type);
                return;
            }
            intentos++;
        }
    }

    bool CanPlace(int startX, int startY, int w, int h)
    {
        for (int x = startX; x < startX + w; x++)
        {
            for (int y = startY; y < startY + h; y++)
            {
                if (IsCellOccupied(x, y)) return false;
            }
        }
        return true;
    }

    void PlaceEntity(int startX, int startY, int w, int h, string type)
    {
        for (int x = startX; x < startX + w; x++)
        {
            for (int y = startY; y < startY + h; y++)
            {
                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
                GameObject go = null;

                if (type == "Island")
                {
                    go = UnityEngine.Object.Instantiate(islandPrefab, pos, Quaternion.identity);
                    if (!go.GetComponent<Obstacle>()) go.AddComponent<Obstacle>();
                }
                else if (type == "Slow") go = UnityEngine.Object.Instantiate(slowPrefab, pos, Quaternion.identity);
                else if (type == "Hazard") go = UnityEngine.Object.Instantiate(hazardPrefab, pos, Quaternion.identity);

                if (go != null) go.name = $"{type}_{x}_{y}";
            }
        }
    }

    // --- SPAWN DE BARCOS RESTRINGIDO ---
    void SpawnPlayerFixed()
    {
        // Columna 0, Filas 1 a 7 (7 filas centrales de un grid de 9)
        int ry = UnityEngine.Random.Range(1, 8);
        SpawnShip(allyPrefab, "FS", 0, ry, false, true);
    }

    void SpawnEnemyFixed()
    {
        // Última columna, Filas 1 a 7
        int ry = UnityEngine.Random.Range(1, 8);
        SpawnShip(enemyPrefab, "ES", rows - 1, ry, true, false);
    }

    // --- UTILIDADES ---
    bool IsCellOccupied(int gx, int gy)
    {
        Vector3 target = new Vector3(gx * spacing, gy * spacing, 0);
        // Usamos distancias pequeñas para detectar barcos, islas, lodo o trampas
        Collider2D hit = Physics2D.OverlapCircle(target, 0.1f);
        // Si no usas colliders, mantenemos el método de búsqueda de objetos:

        Obstacle[] obs = UnityEngine.Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        foreach (var o in obs) if (Vector3.Distance(o.transform.position, target) < 0.1f) return true;

        ShipMovement[] ships = UnityEngine.Object.FindObjectsByType<ShipMovement>(FindObjectsSortMode.None);
        foreach (var s in ships) if (Vector3.Distance(s.transform.position, target) < 0.1f) return true;

        SlowCell[] slows = UnityEngine.Object.FindObjectsByType<SlowCell>(FindObjectsSortMode.None);
        foreach (var sl in slows) if (Vector3.Distance(sl.transform.position, target) < 0.1f) return true;

        HazardCell[] hazs = UnityEngine.Object.FindObjectsByType<HazardCell>(FindObjectsSortMode.None);
        foreach (var h in hazs) if (Vector3.Distance(h.transform.position, target) < 0.1f) return true;

        return false;
    }

    GameObject SpawnShip(GameObject prefab, string name, int gx, int gy, bool isEnemy, bool isPlayer)
    {
        Vector3 pos = new Vector3(gx * spacing, gy * spacing, 0);
        GameObject ship = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity);
        ship.name = name;
        ShipMovement mov = ship.GetComponent<ShipMovement>();
        if (mov != null)
        {
            mov.gridX = gx; mov.gridY = gy; mov.spacing = spacing;
            mov.isPlayerControlled = isPlayer;
            if (isPlayer)
            {
                TurnManager.Instance.playerMovement = mov;
                TurnManager.Instance.playerCombat = ship.GetComponent<ShipCombat>();
            }
            else
            {
                TurnManager.Instance.enemyMovement = mov;
            }
        }
        ShipStats stats = ship.GetComponent<ShipStats>();
        if (stats != null) stats.esEnemigo = isEnemy;
        return ship;
    }
}