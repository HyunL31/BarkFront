using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 20;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dog"))
        {
            DogHealth dogHealth = collision.GetComponent<DogHealth>();
            if (dogHealth != null)
            {
                dogHealth.TakeDamage(attackDamage);
                Debug.Log("Dog attacked! Current Health: " + dogHealth.GetCurrentHealth());
            }
        }
    }

}
