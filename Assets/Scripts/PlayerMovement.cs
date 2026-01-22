using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float MoveSpeed;
    public Vector3 target;
    private bool moving = true;
    void Start()
    {
        target = transform.position;
    }
    public void MoveTo(Vector2 position)
    {
       if (!moving) return;

        target = position;
        moving = false;
    }

    // Update is called once per frame
    void Update()
  {
        if (!moving)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);

           
            if (Vector2.Distance(transform.position, target) < 0.01f)
            {
                transform.position = target; 
                moving = true;
            }
        }
    }
    
}
