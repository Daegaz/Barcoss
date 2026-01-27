using UnityEngine;
using TMPro;

public class UImamapinga : MonoBehaviour
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
            string infoAliado = (statsFS != null) ? $"Casco: {statsFS.shipHealth:F0}" : "HUNDIDO";
            string infoEnemigo = (statsES != null) ? $"Casco: {statsES.shipHealth:F0}" : "HUNDIDO";

            textoUI.text = $"<b>ALIADO (FS)</b>\n{infoAliado}\n\n" +
                           $"<b>ENEMIGO (ES)</b>\n{infoEnemigo}";
        }
    }
}