using UnityEngine;

public class StartingPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = SaveData.Instance.startPosition;
    }
}
