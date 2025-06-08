using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ✅ 체력바 Image 사용을 위한 추가

public class RobotController : MonoBehaviour
{
    public float speed = -1f;
    public float leftBoundaryX = -10f;

    [Header("Combat Settings")]
    public int maxHP = 5;
    public int attackDamage = 1;
    public float attackInterval = 0.5f; // 공격 간격

    private int currentHP;

    [Header("HP Bar")]
    public Image hpBarImage; // ✅ 체력바 연결

    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    private Coroutine attackCoroutine;
    private DogController targetDog;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        currentHP = maxHP;
        UpdateHPBar();
    }

    void Update()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            MoveLeft();
        }

        if (transform.position.x <= leftBoundaryX)
        {
            EndGame();
        }
    }

    void MoveLeft()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("dog"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", true);
            }

            targetDog = other.GetComponent<DogController>();
            if (targetDog != null && attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackDogOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("dog") && !isDead)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            targetDog = null;
        }
    }

IEnumerator AttackDogOverTime()
    {
        while (!isDead && targetDog != null)
        {
            targetDog.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval); // ✅ 변수 사용
        }
    }

    // ✅ 외부에서 데미지를 받을 수 있도록 함수 추가
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = (float)currentHP / maxHP;
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        isAttacking = false;
        animator.SetBool("isAttacking", false);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        Destroy(gameObject, 2f);
    }

    void EndGame()
    {
        Debug.Log("Game Over - Robot reached the left edge.");
        Time.timeScale = 0;
    }
}
