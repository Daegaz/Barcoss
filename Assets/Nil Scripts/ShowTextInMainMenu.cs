using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject mainMenuElements; // El título y el botón de Play actual
    public GameObject introPanel;       // El panel con el fondo y el texto
    public GameObject startButton;      // El botón que quieres que aparezca después

    void Start()
    {
        // Estado inicial: Menú ON, Intro y Botón Start OFF
        if (mainMenuElements != null) mainMenuElements.SetActive(true);
        if (introPanel != null) introPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(false);
    }

    // Este método lo llamará el botón "PLAY" del menú principal
    public void AbrirIntro()
    {
        if (mainMenuElements != null) mainMenuElements.SetActive(false);

        // Activamos el panel (que ahora tiene el fondo delante de Odysea)
        if (introPanel != null) introPanel.SetActive(true);

        // Hacemos aparecer el botón de Start
        if (startButton != null) startButton.SetActive(true);
    }

    // Este método lo llamará el botón "START" (el nuevo)
    public void EmpezarJuego()
    {
        // Aquí metes la lógica de carga de nivel que ya tenías
        Debug.Log("Zarpando hacia la aventura...");
    }
}