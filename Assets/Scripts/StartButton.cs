using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject Button;
    public void DeactivateObjects()
    {
        Destroy(Button);
    }
}
