using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static bool IsCellOccupied(Vector3 targetPos)
    {
        // Buscamos todos los objetos que tengan el script GridEntity
        GridEntity[] entities = Object.FindObjectsByType<GridEntity>(FindObjectsSortMode.None);

        foreach (GridEntity entity in entities)
        {
            // Calculamos la distancia entre el destino y cada entidad encontrada
            if (Vector3.Distance(entity.transform.position, targetPos) < 0.1f)
            {
                if (entity.isObstacle)
                {
                    Debug.Log("Bloqueado por: " + entity.gameObject.name);
                    return true;
                }
            }
        }
        return false;
    }
}