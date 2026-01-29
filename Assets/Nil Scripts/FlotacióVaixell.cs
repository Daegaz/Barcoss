using UnityEngine;

public class FlotacioVaixell : MonoBehaviour
{
    [Header("Ajustaments de moviment")]
    public float amplitud = 15f;  // Quanta distància puja i baixa
    public float frecuencia = 2f; // Com de ràpid ho fa

    private Vector3 posicioInicial;

    void Start()
    {
        // Guardem la posició on l'has posat a l'editor
        posicioInicial = transform.localPosition;
    }

    void Update()
    {
        // Calculem la nova posició Y fent servir el temps actual
        float nouY = posicioInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;

        // Apliquem el canvi sense tocar la X ni la Z
        transform.localPosition = new Vector3(posicioInicial.x, nouY, posicioInicial.z);
    }
}