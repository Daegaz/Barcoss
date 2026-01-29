using UnityEngine;
using System.Collections;

public class MusicFadeManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Configuración de Sonido")]
    public float duracionFade = 2.0f; // Tiempo que tarda en llegar al volumen máximo
    public float volumenMaximo = 1.0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Iniciamos el sonido con volumen 0 y lanzamos el Fade In
        audioSource.volume = 0;
        audioSource.Play();
        StartCoroutine(DoFadeIn());
    }

    IEnumerator DoFadeIn()
    {
        float tiempoTranscurrido = 0;

        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            // Aumentamos el volumen gradualmente
            audioSource.volume = Mathf.Lerp(0, volumenMaximo, tiempoTranscurrido / duracionFade);
            yield return null;
        }

        audioSource.volume = volumenMaximo;
    }

    // Al cambiar de escena, los objetos que no son "DontDestroyOnLoad" se destruyen automáticamente.
    // Unity detiene el AudioSource en cuanto el objeto se destruye.
}