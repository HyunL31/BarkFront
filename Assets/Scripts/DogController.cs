using UnityEngine;
using UnityEngine.UI;

public class DogController : MonoBehaviour
{
    public int baseHP = 100;
    public int baseAttack = 10;

    private int currentHP;
    private int currentAttack;

    public Image hpBarImage;

    public bool isBuffed = false;
    public bool isInvincible = false; // 무적 상태 여부

    public AudioSource attackSound;

    void Start()
    {
        // 능력치 초기화
        currentHP = isBuffed ? Mathf.RoundToInt(baseHP * 1.3f) : baseHP;
        currentAttack = isBuffed ? Mathf.RoundToInt(baseAttack * 1.3f) : baseAttack;

        UpdateHPBar();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return; // 무적이면 데미지 무시

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
            hpBarImage.fillAmount = (float)currentHP / baseHP;
        }
    }

    void Die()
    {
        // 죽는 애니메이션 등 추가 가능
        Destroy(gameObject);
    }
}
