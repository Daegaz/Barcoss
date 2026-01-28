using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float shipHealth = 100f;
    public float maxHealth = 100f; // Añadido para calcular porcentaje
    public bool esEnemigo = false;

    public void RecibirDanio(float danio)
    {
        shipHealth -= danio;
        shipHealth = UnityEngine.Mathf.Max(shipHealth, 0);

        UnityEngine.Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth);

        if (shipHealth <= 0)
        {
            Morir();
        }
    }

    // Nueva función para que la IA consulte su estado
    public float ObtenerPorcentajeVida()
    {
        return shipHealth / maxHealth;
    }

    void Morir()
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}