using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float shipHealth; // No le des valor aquí
    public float maxHealth = 100f;
    public bool esEnemigo = false;

    void Start()
    {
        if (!esEnemigo)
        {
            // El jugador saca su vida del SaveData
            if (SaveData.Instance != null)
            {
                shipHealth = SaveData.Instance.HP;
                maxHealth = 1000f; // O lo que quieras que sea el tope
            }
        }
        else
        {
            // El enemigo empieza con vida llena independiente
            shipHealth = 100f;
        }
    }

    public void RecibirDanio(float danio)
    {
        shipHealth -= danio;
        shipHealth = UnityEngine.Mathf.Max(shipHealth, 0);

        // Si es el jugador, actualizamos el SaveData para que sea persistente
        if (!esEnemigo && SaveData.Instance != null)
        {
            SaveData.Instance.HP = (int)shipHealth;
        }

        UnityEngine.Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth);

        if (shipHealth <= 0) Morir();
    }

    public float ObtenerPorcentajeVida()
    {
        return shipHealth / maxHealth;
    }

    void Morir()
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}