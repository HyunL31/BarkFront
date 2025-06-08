using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 애니메이션 스크립트
/// </summary>

public class Player : MonoBehaviour
{
    public float speed;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isSniffing = false;
    private LootChest targetChest;

    public GameObject interactPromptText;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (interactPromptText != null)
        {
            interactPromptText.SetActive(false);
        }
    }

    // 플레이어 이동
    private void Update()
    {
        // 냄새 맡는 중엔 이동 X
        if (isSniffing)
        {
            return;
        }

        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            sr.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            sr.flipX = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        dir.Normalize();
        rb.linearVelocity = speed * dir;

        animator.SetBool("IsMoving", dir.magnitude > 0);

        // E키 입력으로 냄새 맡는 애니메이션 시작
        if (Input.GetKeyDown(KeyCode.E) && targetChest != null && dir == Vector2.zero)
        {
            if (AvoidCCTV.Instance != null && AvoidCCTV.Instance.IsPlayingCCTVGame)
            {
                return;
            }

            isSniffing = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("IsSniffing");
        }
    }

    // Sniff 애니메이션 이벤트 (마지막 프레임에서 호출)
    public void OnSniffEnd()
    {
        isSniffing = false;

        if (targetChest != null)
        {
            targetChest.OpenChest();
            targetChest = null;
        }
    }

    public void SetTargetChest(LootChest chest)
    {
        targetChest = chest;

        bool isMiniGameRunning = BoxGameManager.Instance != null && BoxGameManager.Instance.IsPlayingMiniGame;

        if (interactPromptText != null)
        {
            interactPromptText.SetActive(!isMiniGameRunning && !chest.IsOpened);
        }
    }

    public void ClearTargetChest()
    {
        targetChest = null;

        if (interactPromptText != null)
        {
            interactPromptText.SetActive(false);
        }
    }
}