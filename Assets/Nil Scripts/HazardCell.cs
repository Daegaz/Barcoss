using UnityEngine;
public class HazardCell : MonoBehaviour
{
    public float damageAmount = 15f;
    public void AplicarDanio(GameObject ship)
    {
        var stats = ship.GetComponent<ShipStats>();
        if (stats != null) stats.RecibirDanio(damageAmount);
    }
}