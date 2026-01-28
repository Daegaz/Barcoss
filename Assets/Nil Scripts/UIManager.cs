using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public ShipStats statsFS;
    public ShipStats statsES;
    public TextMeshProUGUI textoUI;

    void Update()
    {
        if (statsFS == null)
        {
            GameObject obj = GameObject.Find("FS");
            if (obj) statsFS = obj.GetComponent<ShipStats>();
        }
        if (statsES == null)
        {
            GameObject obj = GameObject.Find("ES");
            if (obj) statsES = obj.GetComponent<ShipStats>();
        }

        if (textoUI != null)
        {
            string infoAliado = (statsFS != null) ? $"Integritat: {statsFS.shipHealth:F0}" : "ENFONSAT";
            string infoEnemigo = (statsES != null) ? $"Integritat: {statsES.shipHealth:F0}" : "ENFONSAT";

            textoUI.text = $"<b>Tu</b>\n{infoAliado}\n\n" +
                           $"<b>Enemic</b>\n{infoEnemigo}";
        }
    }
}