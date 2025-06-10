using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// CCTV 로봇 이벤트 스크립트
/// </summary>

public class RobotEvent : MonoBehaviour
{
    public GameObject eventPanel;
    public GameObject cctvPanel;

    private CanvasGroup canvasGroup;
    private List<float> eventTimings = new List<float>();
    private int eventIndex = 0;

    [SerializeField] private GameObject avoidCCTV;

    private bool isEventActive = false;

    public Action OnGameEnded;               // CCTV 게임이 끝났을 때 호출될 콜백

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

    // 낮 시간 동안 호출되어 랜덤 이벤트 시간을 설정
    public void SetupRandomEvents(float dayDuration)
    {
        eventTimings.Clear();
        eventIndex = 0;

        int eventCount = Random.Range(1, 4); // 1~3개의 랜덤 이벤트

        for (int i = 0; i < eventCount; i++)
        {
            float randomTime = Random.Range(5f, dayDuration - 13f); // 시작 후 5초 뒤부터 끝나기 13초 전까지

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

    // 이벤트 트리거 함수 & 한 번에 하나만 실행
    void TriggerEvent()
    {
        if (isEventActive)
        {
            return;
        }

        isEventActive = true;
        eventPanel.SetActive(true);
        StartCoroutine(FlashPanelAndStartGame());
    }

    // 알림 점멸 후 미니게임 패널 열기
    IEnumerator FlashPanelAndStartGame()
    { 
        int flashCount = 3;
        float flashSpeed = 0.25f;

        // 알림 점멸
        for (int i = 0; i < flashCount; i++)
        {
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(flashSpeed);

            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(flashSpeed);
        }

        // 타이밍바 미니게임 강제 종료
        if (BoxGameManager.Instance != null)
        {
            BoxGameManager.Instance.ForceClose();
        }

        // 미니게임 패널 열기
        eventPanel.SetActive(false);
        cctvPanel.SetActive(true);

        if (avoidCCTV != null)
        {
            avoidCCTV.SetActive(false);
            avoidCCTV.SetActive(true);

            OnGameEnded = () => { isEventActive = false; };
        }
    }
}
