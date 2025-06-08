using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    [Tooltip("죽음 애니메이션 트리거 이름")]
    public string deathTrigger = "Die";
    [Tooltip("리스폰 지점 (씬에 빈 GameObject로 두고 참조)")]
    public Transform respawnPoint;
    [Tooltip("리스폰까지 대기 시간")]
    public float respawnDelay = 1.0f;
    [Tooltip("사망 시 차감될 코인")]
    public int deathCoinPenalty = 50;

    Animator animator;
    MonoBehaviour movementScript;
    OxygenSystem oxySystem;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementScript = GetComponent<Player>(); // 플레이어 이동관련 스크립트
        oxySystem = GetComponent<OxygenSystem>();
    }

    public void Die()
    {
        // 1) 이동 비활성
        movementScript.enabled = false;

        // 2) 죽음 애니메이션
        animator.SetTrigger(deathTrigger);

        // 3) 코인 차감
        

        // 4) 리스폰 코루틴
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // 5) 위치 이동
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        // 6) 산소 시스템 리셋
        if (oxySystem != null)
        {
            oxySystem.ResetOxygen();
            oxySystem.enabled = true;
        }

        // 7) 이동 재활성 & 애니메이터 초기화
        movementScript.enabled = true;

        animator.ResetTrigger(deathTrigger);
        animator.Play("Stand"); // Stand 상태 이름으로 바꿔주세요
    }
}
