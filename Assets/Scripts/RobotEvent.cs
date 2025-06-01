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

    public void SetupRandomEvents(float dayDuration)
    {
        eventTimings.Clear();
        eventIndex = 0;

        int eventCount = Random.Range(1, 4);

        for (int i = 0; i < eventCount; i++)
        {
            float randomTime = Random.Range(5f, dayDuration - 5f);
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

    void TriggerEvent()
    {
        Debug.Log("이벤트 발생");
        eventPanel.SetActive(true);
        StartCoroutine(FlashPanelAndStartGame());
    }

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
            avoidCCTV.SetActive(true);
        }

        Debug.Log("cctv 게임 시작");
    }
}
