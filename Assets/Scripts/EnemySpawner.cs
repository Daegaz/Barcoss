using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject[] _enemyPrefabs; 

    
    [SerializeField] private int _enemiesToSpawn = 3;

    void Start()
    {
        
        Invoke("SpawnEnemies", 0.1f);
    }

    public void SpawnEnemies()
    {
        List<Vector2> occupiedPositions = new List<Vector2>();

        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Vector2 randomPos = GetRandomGridPosition();

            
            int attempts = 0;
            while (occupiedPositions.Contains(randomPos) && attempts < 100)
            {
                randomPos = GetRandomGridPosition();
                attempts++;
            }

            occupiedPositions.Add(randomPos);
            SpawnEnemyAt(randomPos);
        }
    }

    private Vector2 GetRandomGridPosition()
    {
        
        int x = Random.Range(0, _gridManager._width);
        int y = Random.Range(0, _gridManager._height);
        return new Vector2(x, y);
    }

    private void SpawnEnemyAt(Vector2 pos)
    {
        GameObject prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        
        
        Vector3 spawnPos = new Vector3(pos.x, pos.y, -1f); 
        
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
