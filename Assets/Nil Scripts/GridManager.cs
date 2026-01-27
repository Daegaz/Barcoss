using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static bool IsCellOccupied(Vector3 targetPos)
    {
        // Buscamos todas las entidades en el mapa
        GridEntity[] entities = Object.FindObjectsByType<GridEntity>(FindObjectsSortMode.None);

        foreach (GridEntity entity in entities)
        {
            // Si la entidad es un obstáculo y está en la misma posición (con margen de error)
            if (entity.isObstacle && Vector3.Distance(entity.transform.position, targetPos) < 0.2f)
            {
                return true;
            }
        }
        return false;
    }
}