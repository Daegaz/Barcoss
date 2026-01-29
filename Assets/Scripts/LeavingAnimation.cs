using UnityEngine;

public class LeavingAnimation : MonoBehaviour
{
    public static LeavingAnimation instance;
    public float speed = 1.5f;
    public void MovementAnimation(Transform target)
    {
        target.position += Vector3.right * speed * Time.deltaTime;
        
    }
}
