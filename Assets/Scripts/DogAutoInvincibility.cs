using System.Collections;
using UnityEngine;

public class DogAutoInvincibility : MonoBehaviour
{
    public float interval = 5f;             // 5초마다
    public float invincibleDuration = 1.5f; // 1.5초간 무적
    public GameObject immuneEffect;         // 무적 이펙트 (자식 오브젝트 등)

    private DogController dogController;

    void Start()
    {
        dogController = GetComponent<DogController>();
        if (dogController == null)
        {
            Debug.LogError("DogController가 필요합니다!");
            enabled = false;
            return;
        }

        StartCoroutine(InvincibilityLoop());
    }

    IEnumerator InvincibilityLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // 무적 시작
            dogController.isInvincible = true;
            if (immuneEffect != null) immuneEffect.SetActive(true);

            yield return new WaitForSeconds(invincibleDuration);

            // 무적 해제
            dogController.isInvincible = false;
            if (immuneEffect != null) immuneEffect.SetActive(false);
        }
    }
}
