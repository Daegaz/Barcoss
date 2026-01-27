using UnityEngine;
public class DestroyAfterTime : MonoBehaviour
{
    void Start() => Destroy(gameObject, 1.0f);
}