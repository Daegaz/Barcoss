using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject allyPrefab; // Arrastra aquí a FS
    public GameObject enemyPrefab; // Arrastra aquí a ES

    public int rows = 10;
    public int cols = 10;
    public float spacing = 1.1f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);

                // Creamos la celda de fondo
                Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                // Lógica para los barcos
                if (x == 1 && y == 4)
                {
                    GameObject ship = Instantiate(allyPrefab, pos, Quaternion.identity);
                    ship.name = "FS"; // Aseguramos el nombre
                }
                else if (x == 7 && y == 4)
                {
                    GameObject ship = Instantiate(enemyPrefab, pos, Quaternion.identity);
                    ship.name = "ES"; // Aseguramos el nombre
                    ShipStats stats = ship.GetComponent<ShipStats>();
                    if (stats != null) stats.esEnemigo = true;

                }
            }
        }
    }
}