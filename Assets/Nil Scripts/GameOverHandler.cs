using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public GameObject botonSalir;

    public void ActivarBotonMuerte()
    {
        if (botonSalir != null)
        {
            botonSalir.SetActive(true);
        }
    }

    public void CerrarJuego()
    {
        Debug.Log("Cerrando el viaje...");
        Application.Quit(); // Esto cierra el .exe o .app compilado
    }
}