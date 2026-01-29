//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    private AudioSource sfxSource;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            // Creamos un AudioSource dinámicamente si no existe
//            sfxSource = gameObject.AddComponent<AudioSource>();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void PlaySFX(AudioClip clip)
//    {
//        if (clip != null)
//        {
//            // PlayOneShot es genial para 2D porque permite solapar sonidos
//            // Y le añadimos un pequeño cambio de tono (pitch) para que no sea monótono
//            sfxSource.pitch = Random.Range(0.9f, 1.1f);
//            sfxSource.PlayOneShot(clip);
//        }
//    }
//}

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- ESTA ES LA QUE LE FALTA A SHIPCOMBAT ---
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        // Variamos un poco el pitch para que no suene repetitivo
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = 1.0f; // Resetear pitch
    }

    // --- PARA LA EMBESTIDA CON RETRASO ---
    public void PlayDelayedSFX(AudioClip clip, float delay)
    {
        if (clip == null) return;
        StartCoroutine(RoutineDelayedSFX(clip, delay));
    }

    private IEnumerator RoutineDelayedSFX(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.PlayOneShot(clip);
    }

    // --- PARA LAS FLECHAS CON FADE OUT ---
    public void PlaySFXWithFade(AudioClip clip, float duration, float fadeTime)
    {
        if (clip == null) return;
        StartCoroutine(RoutineSFXFade(clip, duration, fadeTime));
    }

    private IEnumerator RoutineSFXFade(AudioClip clip, float duration, float fadeTime)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.Play();

        yield return new WaitForSeconds(Mathf.Max(0, duration - fadeTime));

        float startVolume = tempSource.volume;
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            tempSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeTime);
            yield return null;
        }

        tempSource.Stop();
        Destroy(tempSource);
    }
}