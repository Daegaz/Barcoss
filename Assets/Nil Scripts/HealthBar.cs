using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public ShipStats stats;
    public UnityEngine.UI.Image healthFill;

    void Update()
    {
        if (stats != null && healthFill != null)
        {
            healthFill.fillAmount = stats.ObtenerPorcentajeVida();

            // Cambia de color: Verde si está bien, Rojo si va a morir
            healthFill.color = UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.green, stats.ObtenerPorcentajeVida());
        }
    }
}