using System.Diagnostics;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float shipHealth = 100f;
    public float crewHealth = 100f;
    public bool esEnemigo = false; // Marcar como True en el prefab del enemigo

    public void RecibirDanio(float danioCasco, float danioTripulacion)
    {
        shipHealth -= danioCasco;
        crewHealth -= danioTripulacion;

        shipHealth = Mathf.Max(shipHealth, 0);
        crewHealth = Mathf.Max(crewHealth, 0);

        //Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth + " | Tripulación: " + crewHealth);

        if (shipHealth <= 0 || crewHealth <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
       // Debug.Log(gameObject.name + " ha sido destruido.");
        // Si es el aliado, podrías disparar un "Game Over"
        // Si es enemigo, simplemente se destruye
        Destroy(gameObject);
    }
}