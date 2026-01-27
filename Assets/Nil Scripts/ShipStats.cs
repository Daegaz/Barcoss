using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public float shipHealth = 100f;
    public bool esEnemigo = false;
    public GameObject efectoSangrePrefab;

    public void RecibirDanio(float danio)
    {
        shipHealth -= danio;
        shipHealth = Mathf.Max(shipHealth, 0);

        if (efectoSangrePrefab != null)
        {
            // Ajusta el 0.5f para subir o bajar la partícula a tu gusto
            Vector3 posicionAjustada = transform.position + new Vector3(0, 0, -1f);

            // Si tu juego es 2D y el fondo está en Z=0, pon el ajuste en Z=-1 para que esté "delante"
            // Si tu juego es 3D (vista cenital), usa: new Vector3(0, 1f, 0)

            GameObject vfx = Instantiate(efectoSangrePrefab, posicionAjustada, Quaternion.identity);

            // Destruye la partícula después de un segundo
            Destroy(vfx, 1.0f);
        }

        if (shipHealth <= 0) Morir();
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}