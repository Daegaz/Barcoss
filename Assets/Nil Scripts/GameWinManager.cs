using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI mensajeText;
    public GameObject botonReiniciar;

    void Start()
    {
        // Al empezar, nos aseguramos de que TODO esté apagado
        if (mensajeText != null) mensajeText.gameObject.SetActive(false);
        if (botonReiniciar != null) botonReiniciar.SetActive(false);
    }

    void Update()
    {
        // 1. Verificar si el jugador (FS) ha muerto
        if (GameObject.Find("FS") == null)
        {
            MostrarFinPartida("¡HAS SIDO HUNDIDO!", Color.red);
        }

        // 2. Verificar si el enemigo (ES) ha muerto
        if (GameObject.Find("ES") == null)
        {
            MostrarFinPartida("¡VICTORIA GRIEGA!", Color.yellow);
        }
    }

    void MostrarFinPartida(string texto, Color color)
    {
        if (mensajeText != null)
        {
            mensajeText.text = texto;
            mensajeText.color = color;
            mensajeText.gameObject.SetActive(true);
        }

        if (botonReiniciar != null)
        {
            botonReiniciar.SetActive(true);
        }
    }

    public void ReiniciarJuego()
    {
        UnityEngine.Debug.Log("¡BOTÓN PULSADO CORRECTAMENTE!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}