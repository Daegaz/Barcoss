using UnityEngine;

public class RepairButton : MonoBehaviour
{
   private void OnMouseDown()
    {
        if(SaveData.Instance.resources >= 5)
        {
            SaveData.Instance.AddResources(-5);
            SaveData.Instance.TakeDamage(5);
        }
    }
}
