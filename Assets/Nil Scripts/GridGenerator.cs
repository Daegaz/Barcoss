using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject allyPrefab;
    public GameObject enemyPrefab;
    public GameObject islandPrefab;
    public int rows = 9;
    public int cols = 9;
    public float spacing = 1.0f;

    void Start() => GenerateGrid();

    void GenerateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                // Volvemos a la posición normal sin restas raras
                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
                Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                // Coordenadas solicitadas: (4,5), (5,5), (4,6), (5,6)
                if ((x == 4 || x == 5) && (y == 4 || y == 5))
                {
                    SpawnObstacle(x, y);
                }
                else if (x == 1 && y == 4)
                {
                    GameObject ally = SpawnShip(allyPrefab, "FS", x, y, false);
                    ShipMovement mov = ally.GetComponent<ShipMovement>();
                    mov.isPlayerControlled = true;

                    TurnManager.Instance.playerMovement = mov;
                    TurnManager.Instance.playerCombat = ally.GetComponent<ShipCombat>();
                }
                else if (x == 7 && y == 4)
                {
                    GameObject enemy = SpawnShip(enemyPrefab, "ES", x, y, true);
                    ShipMovement mov = enemy.GetComponent<ShipMovement>();
                    mov.isPlayerControlled = false;

                    TurnManager.Instance.enemyMovement = mov;
                }
            }
        }
    }

    void SpawnObstacle(int gx, int gy)
    {
        Vector3 pos = new Vector3(gx * spacing, gy * spacing, 0);
        GameObject island = Instantiate(islandPrefab, pos, Quaternion.identity);
        island.name = $"Island_{gx}_{gy}";

        if (island.GetComponent<Obstacle>() == null)
        {
            island.AddComponent<Obstacle>();
        }
    }

    GameObject SpawnShip(GameObject prefab, string name, int gx, int gy, bool isEnemy)
    {
        Vector3 pos = new Vector3(gx * spacing, gy * spacing, 0);
        GameObject ship = Instantiate(prefab, pos, Quaternion.identity);
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