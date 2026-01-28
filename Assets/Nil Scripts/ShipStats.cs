using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float shipHealth = 100f;
    public bool esEnemigo = false;

    public void RecibirDanio(float danio)
    {
        shipHealth -= danio;
        shipHealth = Mathf.Max(shipHealth, 0);

        // Forzamos el uso de UnityEngine aquí
        UnityEngine.Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth);

        if (shipHealth <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}