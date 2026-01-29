using UnityEngine;

public class MusicManagerPersistent : MonoBehaviour
{
    private static MusicManagerPersistent instancia;

    void Awake()
    {
        // Sistema Singleton: si ya existe una música sonando, destruye esta nueva
        if (instancia != null && instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Si es la primera vez que se crea, se marca como persistente
        instancia = this;
        DontDestroyOnLoad(this.gameObject);
    }
}