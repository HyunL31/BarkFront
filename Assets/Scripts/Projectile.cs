using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public Vector2 direction = Vector2.right;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("robot"))
        {
            RobotController robot = other.GetComponent<RobotController>();
            if (robot != null)
            {
                robot.TakeDamage(damage);
            }

            
            Destroy(gameObject);
        }
    }
}
