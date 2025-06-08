using UnityEngine;

public class RangedDogController : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 8f;
    public float fireRate = 1f;
    public int attackDamage = 1;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public Transform firePoint; // 총알 발사 위치

    private float lastFireTime;

void Update()
{
    float rayYOffset = 0.5f; // 원하는 만큼 y축으로 올림
    Vector3 rayOrigin = transform.position + Vector3.up * rayYOffset;

    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, attackRange, LayerMask.GetMask("Robot"));
    if (hit.collider != null)
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }
    }

    // Debug용 Ray 그리기
    Debug.DrawRay(rayOrigin, Vector2.right * attackRange, Color.red);
}
    void FireProjectile()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.damage = attackDamage;
            projectile.speed = projectileSpeed;
            projectile.direction = Vector2.right;
        }
    }
}
