using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    private float speed = -1f;
    public float leftBoundaryX = -10f;

    [Header("Combat Settings")]
    public int maxHP = 5;
    public int attackDamage = 1;
    public float attackInterval = 0.5f; // 공격 간격

    private int currentHP;

    [Header("HP Bar")]
    public Image hpBarImage;

    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    private Coroutine attackCoroutine;
    private DogController targetDog;

    public AudioSource attackSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        currentHP = maxHP;
        UpdateHPBar();

        int currentDay = 1;
        if (DayNightTimer.Instance != null)
        {
            currentDay = DayNightTimer.Instance.GetCurrentDay();
        }

        speed = -1f * (1f + (currentDay - 1) * 0.5f) / 2;
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
        if (attackSound != null)
        {
            attackSound.Play();
        }

        while (!isDead && targetDog != null)
        {
            targetDog.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval);
        }
    }

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

        if (DayNightTimer.Instance != null)
            DayNightTimer.Instance.SetBadEndingTriggered();

        SceneManager.LoadScene("BadEnding");
    }
}
