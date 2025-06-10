using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float depletionRate = 5f;
    public float regenRate = 20f;

    private float currentOxygen;
    private bool isInZone;

    public static event Action<float> OnOxygenChanged;

    void Start()
    {
        currentOxygen = maxOxygen;
    }

    void Update()
    {
        float delta = (isInZone ? regenRate : -depletionRate) * Time.deltaTime;
        if (!Mathf.Approximately(delta, 0f))
        {
            float oldO2 = currentOxygen;
            currentOxygen = Mathf.Clamp(currentOxygen + delta, 0f, maxOxygen);
            if (!Mathf.Approximately(oldO2, currentOxygen))
            {
                Debug.Log($"[O2] {(isInZone ? "[����]" : "[�Ҹ�]")} {currentOxygen:F1}/{maxOxygen}");
                OnOxygenChanged?.Invoke(currentOxygen / maxOxygen);
            }

            if (currentOxygen <= 0f)
            {
                HandleOxygenDepleted();
            }
        }
    }

    private void HandleOxygenDepleted()
    {
        Debug.LogWarning("!! ��� 0! ��� ó�� ȣ��");
        var death = GetComponent<PlayerDeath>();
        if (death != null)
        {
            death.Die();
        }
        // ��� �ý����� ���� ó�� �� ������ �ڿ� ResetOxygen()���� �ٽ� Ȱ��ȭ ��
        enabled = false;
    }

    public void ResetOxygen()
    {
        currentOxygen = maxOxygen;
        OnOxygenChanged?.Invoke(1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OxygenZone"))
            isInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OxygenZone"))
            isInZone = false;
    }
}
