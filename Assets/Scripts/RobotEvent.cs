using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotEvent : MonoBehaviour
{
    public GameObject eventPanel;
    public GameObject cctvPanel;

    private CanvasGroup canvasGroup;
    private List<float> eventTimings = new List<float>();
    private int eventIndex = 0;

    [SerializeField] private GameObject avoidCCTV;

    private bool isEventActive = false;

    public System.Action OnGameEnded;               // CCTV ������ ������ �� ȣ��� �ݹ�

    void Awake()
    {
        canvasGroup = eventPanel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = eventPanel.AddComponent<CanvasGroup>();
        }

        // ó������ ��� ����
        canvasGroup.alpha = 0f;
        eventPanel.SetActive(false);
        cctvPanel.SetActive(false);
    }

    // �� �ð� ���� ȣ��Ǿ� ���� �̺�Ʈ �ð��� ����
    public void SetupRandomEvents(float dayDuration)
    {
        eventTimings.Clear();
        eventIndex = 0;

        int eventCount = Random.Range(1, 4); // 1~3���� ���� �̺�Ʈ

        for (int i = 0; i < eventCount; i++)
        {
            float randomTime = Random.Range(5f, dayDuration - 5f); // 5�� ~ ��-5�� ����
            eventTimings.Add(randomTime);
        }

        eventTimings.Sort(); // �ð� �� ����
    }

    // �� ������ �� �ð� Ÿ�̸ӷκ��� ȣ�� & �̺�Ʈ �ð� ���� �� Ʈ����
    public void CheckEventTrigger(float currentTime, float dayDuration)
    {
        if (eventIndex < eventTimings.Count && currentTime >= eventTimings[eventIndex])
        {
            TriggerEvent();
            eventIndex++;
        }
    }

    // ���� �ǰų� �ʱ�ȭ �ʿ� �� ȣ��
    public void ResetEvents()
    {
        StopAllCoroutines();              // ���� ���� ���� ȿ�� ����
        eventPanel.SetActive(false);
        cctvPanel.SetActive(false);
        canvasGroup.alpha = 0f;           // ���� ���� �ʱ�ȭ

        eventTimings.Clear();             // �̺�Ʈ �ð� ��� �ʱ�ȭ
        eventIndex = 0;
    }

    // �̺�Ʈ Ʈ���� �Լ� & �� ���� �ϳ��� ����ǵ��� ��
    void TriggerEvent()
    {
        if (isEventActive) return; // �ߺ� ���� ����

        Debug.Log("�̺�Ʈ �߻�");
        isEventActive = true;      // �̺�Ʈ �� ���·� ����
        eventPanel.SetActive(true);
        StartCoroutine(FlashPanelAndStartGame()); // ������ �� �̴ϰ��� ����
    }

    // �˸� ���� �� �̴ϰ��� �г� ����
    IEnumerator FlashPanelAndStartGame()
    {
        int flashCount = 3;
        float flashSpeed = 0.25f;

        for (int i = 0; i < flashCount; i++)
        {
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(flashSpeed);
            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(flashSpeed);
        }

        eventPanel.SetActive(false);
        cctvPanel.SetActive(true);

        if (avoidCCTV != null)
        {
            avoidCCTV.SetActive(false);
            avoidCCTV.SetActive(true);

            // �̴ϰ����� ������ isEventActive�� false�� �ǵ���
            OnGameEnded = () => { isEventActive = false; };
        }

        Debug.Log("cctv ���� ����");
    }
}
