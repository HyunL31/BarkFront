using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isSniffing = false;
    private LootChest targetChest;

    [Header("UI & Interactions")]
    public GameObject interactPromptText;

    [Header("Oxygen Tank")]
    [SerializeField] private SpriteRenderer tankSr;

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

    private void Update()
    {
        if (isSniffing) return;

        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        if (dir.x != 0)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(dir.x);
            transform.localScale = s;
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
