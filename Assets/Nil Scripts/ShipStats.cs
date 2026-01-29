//using UnityEngine;

//public class ShipStats : MonoBehaviour
//{
//    public float shipHealth; // No le des valor aquí
//    public float maxHealth = 100f;
//    public bool esEnemigo = false;

//    void Start()
//    {
//        if (!esEnemigo)
//        {
//            // El jugador saca su vida del SaveData
//            if (SaveData.Instance != null)
//            {
//                shipHealth = SaveData.Instance.HP;
//                maxHealth = 1000f; // O lo que quieras que sea el tope
//            }
//        }
//        else
//        {
//            // El enemigo empieza con vida llena independiente
//            shipHealth = 100f;
//        }
//    }

//    public void RecibirDanio(float danio)
//    {
//        shipHealth -= danio;
//        shipHealth = UnityEngine.Mathf.Max(shipHealth, 0);

//        // Si es el jugador, actualizamos el SaveData para que sea persistente
//        if (!esEnemigo && SaveData.Instance != null)
//        {
//            SaveData.Instance.HP = (int)shipHealth;
//        }

//        UnityEngine.Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth);

//        if (shipHealth <= 0) Morir();
//    }

//    public float ObtenerPorcentajeVida()
//    {
//        return shipHealth / maxHealth;
//    }

//    void Morir()
//    {
//        UnityEngine.Object.Destroy(gameObject);
//    }
//}


using UnityEngine;
using System.Collections;

public class ShipStats : MonoBehaviour
{
    public float shipHealth;
    public float maxHealth = 100f;
    public bool esEnemigo = false;

    [Header("Efectos Visuales")]
    private SpriteRenderer spriteRenderer;
    public Color colorFlash = Color.red;
    public float duracionFlash = 0.15f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!esEnemigo)
        {
            if (SaveData.Instance != null)
            {
                shipHealth = SaveData.Instance.HP;
                maxHealth = 1000f;
            }
        }
        else
        {
            shipHealth = 100f;
        }
    }

    public void RecibirDanio(float danio)
    {
        shipHealth -= danio;
        shipHealth = Mathf.Max(shipHealth, 0);

        // Guardar persistencia si es el jugador
        if (!esEnemigo && SaveData.Instance != null)
        {
            SaveData.Instance.HP = (int)shipHealth;
        }

        // Ejecutar el parpadeo
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashDeDano());
        }

        Debug.Log(gameObject.name + " recibió daño. Casco: " + shipHealth);

        if (shipHealth <= 0) Morir();
    }

    IEnumerator FlashDeDano()
    {
        // Cambia al color de daño
        spriteRenderer.color = colorFlash;

        yield return new WaitForSeconds(duracionFlash);

        // Vuelve al color original (Blanco normal)
        spriteRenderer.color = Color.white;
    }

    public float ObtenerPorcentajeVida()
    {
        return shipHealth / maxHealth;
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}