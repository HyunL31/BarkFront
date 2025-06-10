using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    [Tooltip("���� �ִϸ��̼� Ʈ���� �̸�")]
    public string deathTrigger = "Die";
    [Tooltip("������ ���� (���� �� GameObject�� �ΰ� ����)")]
    public Transform respawnPoint;
    [Tooltip("���������� ��� �ð�")]
    public float respawnDelay = 1.0f;
    [Tooltip("��� �� ������ ����")]
    public int deathCoinPenalty = 50;

    Animator animator;
    MonoBehaviour movementScript;
    OxygenSystem oxySystem;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementScript = GetComponent<Player>(); // �÷��̾� �̵����� ��ũ��Ʈ
        oxySystem = GetComponent<OxygenSystem>();
    }

    public void Die()
    {
        // 1) �̵� ��Ȱ��
        movementScript.enabled = false;

        // 2) ���� �ִϸ��̼�
        animator.SetTrigger(deathTrigger);

        // 3) ���� ����
        

        // 4) ������ �ڷ�ƾ
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // 5) ��ġ �̵�
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        // 6) ��� �ý��� ����
        if (oxySystem != null)
        {
            oxySystem.ResetOxygen();
            oxySystem.enabled = true;
        }

        // 7) �̵� ��Ȱ�� & �ִϸ����� �ʱ�ȭ
        movementScript.enabled = true;

        animator.ResetTrigger(deathTrigger);
        animator.Play("Stand"); // Stand ���� �̸����� �ٲ��ּ���
    }
}
