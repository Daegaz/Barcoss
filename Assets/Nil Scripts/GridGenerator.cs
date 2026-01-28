using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject allyPrefab;
    public GameObject enemyPrefab;
    public GameObject islandPrefab;
    public int rows = 9;
    public int cols = 9;
    public float spacing = 1.1f;

    void Start() => GenerateGrid();

    void GenerateGrid()
    {
        // 1. Generar la cuadrícula base e islas fijas
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
                UnityEngine.Object.Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                // Islas centrales fijas
                if ((x == 4 || x == 5) && (y == 4 || y == 5))
                {
                    SpawnObstacle(x, y);
                }
            }
        }

        // 2. Spawn Aleatorio del Jugador (Primeras 3 columnas: 0, 1, 2)
        SpawnPlayerRandom();

        // 3. Spawn Aleatorio del Enemigo (Últimas 3 columnas: 6, 7, 8)
        SpawnEnemyRandom();
    }

    void SpawnPlayerRandom()
    {
        Vector2Int pos = GetRandomEmptyCell(0, 2, 0, cols - 1);

        GameObject ally = SpawnShip(allyPrefab, "FS", pos.x, pos.y, false);
        ShipMovement mov = ally.GetComponent<ShipMovement>();
        mov.isPlayerControlled = true;

        TurnManager.Instance.playerMovement = mov;
        TurnManager.Instance.playerCombat = ally.GetComponent<ShipCombat>();

        UnityEngine.Debug.Log($"[SPAWN] Aliado en: ({pos.x}, {pos.y})");
    }

    void SpawnEnemyRandom()
    {
        Vector2Int pos = GetRandomEmptyCell(rows - 3, rows - 1, 0, cols - 1);

        GameObject enemy = SpawnShip(enemyPrefab, "ES", pos.x, pos.y, true);
        ShipMovement mov = enemy.GetComponent<ShipMovement>();
        mov.isPlayerControlled = false;

        TurnManager.Instance.enemyMovement = mov;

        UnityEngine.Debug.Log($"[SPAWN] Enemigo en: ({pos.x}, {pos.y})");
    }

    Vector2Int GetRandomEmptyCell(int minX, int maxX, int minY, int maxY)
    {
        int intentos = 0;
        while (intentos < 100)
        {
            int rx = UnityEngine.Random.Range(minX, maxX + 1);
            int ry = UnityEngine.Random.Range(minY, maxY + 1);

            if (!IsCellOccupied(rx, ry))
            {
                return new Vector2Int(rx, ry);
            }
            intentos++;
        }

        UnityEngine.Debug.LogWarning("No se encontró celda vacía tras 100 intentos.");
        return new Vector2Int(minX, minY);
    }

    bool IsCellOccupied(int gx, int gy)
    {
        Vector3 targetPos = new Vector3(gx * spacing, gy * spacing, 0);

        // Comprobamos obstáculos (islas)
        Obstacle[] obstacles = UnityEngine.Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        foreach (Obstacle o in obstacles)
        {
            if (Vector3.Distance(o.transform.position, targetPos) < 0.1f) return true;
        }

        // Comprobamos otros barcos
        ShipMovement[] ships = UnityEngine.Object.FindObjectsByType<ShipMovement>(FindObjectsSortMode.None);
        foreach (ShipMovement s in ships)
        {
            if (Vector3.Distance(s.transform.position, targetPos) < 0.1f) return true;
        }

        return false;
    }

    void SpawnObstacle(int gx, int gy)
    {
        Vector3 pos = new Vector3(gx * spacing, gy * spacing, 0);
        GameObject island = UnityEngine.Object.Instantiate(islandPrefab, pos, Quaternion.identity);
        island.name = $"Island_{gx}_{gy}";

        if (island.GetComponent<Obstacle>() == null)
        {
            island.AddComponent<Obstacle>();
        }
    }

    GameObject SpawnShip(GameObject prefab, string name, int gx, int gy, bool isEnemy)
    {
        Vector3 pos = new Vector3(gx * spacing, gy * spacing, 0);
        GameObject ship = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity);
        ship.name = name;

        ShipMovement mov = ship.GetComponent<ShipMovement>();
        if (mov != null)
        {
            mov.spacing = spacing;
            mov.gridX = gx;
            mov.gridY = gy;
        }

        ShipStats stats = ship.GetComponent<ShipStats>();
        if (stats != null) stats.esEnemigo = isEnemy;

        return ship;
    }
}