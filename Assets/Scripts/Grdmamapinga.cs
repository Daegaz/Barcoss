using System.Collections.Generic;
using UnityEngine;

public class Grdmamapinga : MonoBehaviour
{
    [SerializeField] public int _width = 10, _height = 10;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        if (_cam != null)
        {
            _cam.position = new Vector3((_width - 1) * 0.5f, (_height - 1) * 0.5f, -10f);
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        _tiles.TryGetValue(pos, out Tile tile);
        return tile;
    }
}