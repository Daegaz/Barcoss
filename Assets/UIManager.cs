using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public ShipStats statsFS; // Arrastra el barco aliado aquí en el Inspector
    public ShipStats statsES; // Arrastra el barco enemigo aquí en el Inspector
    public TextMeshProUGUI textoUI;

    void Update()
    {
        // Si faltan referencias, intentamos buscarlas una vez
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

        if (statsFS != null && statsES != null && textoUI != null)
        {
            textoUI.text = $"<b>ALIADO (FS)</b>\nCasco: {statsFS.shipHealth:F0}\nTripulación: {statsFS.crewHealth:F0}\n\n" +
                           $"<b>ENEMIGO (ES)</b>\nCasco: {statsES.shipHealth:F0}\nTripulación: {statsES.crewHealth:F0}";
        }
    }
}