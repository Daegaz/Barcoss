//using System.Diagnostics;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameWinManager : MonoBehaviour
//{
//    [Header("Referencias de UI")]
//    public TextMeshProUGUI mensajeText;
//    public GameObject botonReiniciar;
//    [SerializeField] private LevelLoader LevelLoader;

//    [SerializeField] private ShipStats shipStats;

//    void Start()
//    {
//        // Al empezar, nos aseguramos de que TODO esté apagado
//        if (mensajeText != null) mensajeText.gameObject.SetActive(false);
//        if (botonReiniciar != null) botonReiniciar.SetActive(false);
//    }

//    void Update()
//    {
//        // 1. Verificar si el jugador (FS) ha muerto
//        if (GameObject.Find("FS") == null)
//        {
//            MostrarFinPartida("Fi de la aventura.", Color.red);
//        }

//        // 2. Verificar si el enemigo (ES) ha muerto
//        if (GameObject.Find("ES") == null)
//        {
//            MostrarFinPartida("VICTÒRIA!", Color.yellow);
//        }
//    }

//    void MostrarFinPartida(string texto, Color color)
//    {
//        if (mensajeText != null)
//        {
//            mensajeText.text = texto;
//            mensajeText.color = color;
//            mensajeText.gameObject.SetActive(true);
//        }

//        if (botonReiniciar != null && GameObject.Find("FS") != null)
//        { 
//           botonReiniciar.SetActive(true);
//        }
//    }

//    public void ReiniciarJuego()
//    {
//        //UnityEngine.Debug.Log("¡BOTÓN PULSADO CORRECTAMENTE!");
//        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

//        SaveData.Instance.HP = (int) shipStats.shipHealth;
//        LevelLoader.LoadLevel(2);
//    }

//}


using TMPro;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI mensajeText;
    public GameObject botonReiniciar;
    public GameObject Square;

    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private ShipStats shipStats;

    private bool juegoTerminado = false;

    void Start()
    {
        if (mensajeText != null) mensajeText.gameObject.SetActive(false);
        if (botonReiniciar != null) botonReiniciar.SetActive(false);
        if (Square != null) Square.SetActive(false);
    }

    void Update()
    {
        if (juegoTerminado) return; // Evitamos ejecutar Find cada frame si ya acabó

        // 1. Verificar si el jugador (FS) ha muerto
        if (GameObject.Find("FS") == null)
        {
            juegoTerminado = true;
            MostrarFinPartida("Fi de l'aventura.", Color.red, false);
        }
        // 2. Verificar si el enemigo (ES) ha muerto
        else if (GameObject.Find("ES") == null)
        {
            juegoTerminado = true;
            MostrarFinPartida("L'horitzó ens somriu, però Ítaca encara resta lluny.", Color.white, true);
       
        }
    }

    void MostrarFinPartida(string texto, Color color, bool victoria)
    {
        if (mensajeText != null)
        {
            mensajeText.text = texto;
            mensajeText.color = color;
            mensajeText.gameObject.SetActive(true);
        }

        // El botón solo sale si ganamos (o si quieres que salga siempre, quita el 'victoria')
        if (botonReiniciar != null && GameObject.Find("FS") != null)
        {
            botonReiniciar.SetActive(true);;
        }
        if (Square != null)
        {
            Square.SetActive(true);
        }
    }

    public void ReiniciarJuego()
    {
        // CORRECCIÓN: Si el jugador ha muerto, shipStats es nulo. 
        // Solo guardamos si el objeto todavía existe.
        if (shipStats != null && SaveData.Instance != null)
        {
            SaveData.Instance.HP = (int)shipStats.shipHealth;
            //if (SaveData.Instance.isBossDone == true)
            //    UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
        else if (SaveData.Instance != null)
        {
            // Opcional: Si murió, podrías resetear su vida a un valor base para el reinicio
            // SaveData.Instance.HP = 100; 
        }

        if (levelLoader != null)
        {
            levelLoader.LoadLevel(2);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }
}