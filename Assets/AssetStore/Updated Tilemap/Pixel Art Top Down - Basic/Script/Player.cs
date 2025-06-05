using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class Player : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private bool isSniffing = false;
        private LootChest targetChest;

        private void Start()
        {
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isSniffing) return;  // 냄새 맡는 중엔 이동 X

            Vector2 dir = Vector2.zero;

            if (Input.GetKey(KeyCode.A)) { dir.x = -1; sr.flipX = true; }
            else if (Input.GetKey(KeyCode.D)) { dir.x = 1; sr.flipX = false; }

            if (Input.GetKey(KeyCode.W)) dir.y = 1;
            else if (Input.GetKey(KeyCode.S)) dir.y = -1;

            dir.Normalize();
            rb.linearVelocity = speed * dir;

            animator.SetBool("IsMoving", dir.magnitude > 0);

            // E키 입력으로 냄새 맡기 시작
            if (Input.GetKeyDown(KeyCode.E) && targetChest != null && dir == Vector2.zero)
            {
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
        }

        public void ClearTargetChest()
        {
            if (targetChest != null)
            {
                targetChest = null;
            }
        }
    }
}
