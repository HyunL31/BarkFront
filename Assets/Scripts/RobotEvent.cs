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
            float randomTime = Random.Range(5f, dayDuration - 13f); // 5�� ~ ��-13�� ����
            eventTimings.Add(randomTime);
        }

        eventTimings.Sort();
    }

    public void CheckEventTrigger(float currentTime, float dayDuration)
    {
        if (eventIndex < eventTimings.Count && currentTime >= eventTimings[eventIndex])
        {
            TriggerEvent();
            eventIndex++;
        }
    }

    public void ResetEvents()
    {
        StopAllCoroutines();
        eventPanel.SetActive(false);
        cctvPanel.SetActive(false);
        canvasGroup.alpha = 0f;

        eventTimings.Clear();
        eventIndex = 0;
    }

    // �̺�Ʈ Ʈ���� �Լ� & �� ���� �ϳ��� ����
    void TriggerEvent()
    {
        if (isEventActive)
        {
            return;
        }

        Debug.Log("�̺�Ʈ �߻�");
        isEventActive = true;
        eventPanel.SetActive(true);
        StartCoroutine(FlashPanelAndStartGame());
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

            OnGameEnded = () => { isEventActive = false; };
        }

        Debug.Log("cctv ���� ����");
    }
}
