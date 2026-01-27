using System.Collections.Generic;
using UnityEngine;

public class MovementRange : MonoBehaviour
{
    [Header("Referencia al jugador")]
    [SerializeField] private Transform player;

    [Header("Configuración del rango")]
    [SerializeField] private float rangeRadius = 2f;      // Radio del rango de movimiento
    [SerializeField] private LayerMask tileLayer;         // Layer de las tiles

    // Lista de tiles activas actualmente
    private HashSet<Tile> activeTiles = new HashSet<Tile>();

    private void LateUpdate()
    {
        if (player != null)
            transform.position = player.position;

        UpdateTilesInRange();
    }

    private void UpdateTilesInRange()
    {
        // Detecta todas las tiles dentro del rango
        Collider2D[] tilesInRange = Physics2D.OverlapCircleAll(transform.position, rangeRadius, tileLayer);

        // Nuevo conjunto de tiles detectadas
        HashSet<Tile> tilesDetected = new HashSet<Tile>();

        foreach (Collider2D col in tilesInRange)
        {
            Tile tile = col.GetComponent<Tile>();
            if (tile != null)
            {
                tilesDetected.Add(tile);
                // Activar selección si no estaba activada
                if (!tile.selection.activeSelf)
                    tile.selection.SetActive(true);
            }
        }

        // Desactivar las tiles que estaban activas pero ya no están dentro del rango
        foreach (Tile tile in activeTiles)
        {
            if (!tilesDetected.Contains(tile))
            {
                tile.selection.SetActive(false);
                tile._highlight.SetActive(false);
            }
        }

        // Actualizar lista de tiles activas
        activeTiles = tilesDetected;
    }
}
