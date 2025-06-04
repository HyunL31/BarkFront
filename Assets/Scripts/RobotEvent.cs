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

    public System.Action OnGameEnded;               // CCTV 게임이 끝났을 때 호출될 콜백

    void Awake()
    {
        canvasGroup = eventPanel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = eventPanel.AddComponent<CanvasGroup>();
        }

        // 처음에는 모두 숨김
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
            float randomTime = Random.Range(5f, dayDuration - 5f); // 5초 ~ 끝-5초 사이
            eventTimings.Add(randomTime);
        }

        eventTimings.Sort(); // 시간 순 정렬
    }

    // 매 프레임 낮 시간 타이머로부터 호출 & 이벤트 시간 도달 시 트리거
    public void CheckEventTrigger(float currentTime, float dayDuration)
    {
        if (eventIndex < eventTimings.Count && currentTime >= eventTimings[eventIndex])
        {
            TriggerEvent();
            eventIndex++;
        }
    }

    // 밤이 되거나 초기화 필요 시 호출
    public void ResetEvents()
    {
        StopAllCoroutines();              // 진행 중인 점멸 효과 정지
        eventPanel.SetActive(false);
        cctvPanel.SetActive(false);
        canvasGroup.alpha = 0f;           // 점멸 상태 초기화

        eventTimings.Clear();             // 이벤트 시간 목록 초기화
        eventIndex = 0;
    }

    // 이벤트 트리거 함수 & 한 번에 하나만 실행되도록 함
    void TriggerEvent()
    {
        if (isEventActive) return; // 중복 실행 방지

        Debug.Log("이벤트 발생");
        isEventActive = true;      // 이벤트 중 상태로 설정
        eventPanel.SetActive(true);
        StartCoroutine(FlashPanelAndStartGame()); // 깜빡임 후 미니게임 시작
    }

    // 알림 점멸 후 미니게임 패널 열기
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

            // 미니게임이 끝나면 isEventActive를 false로 되돌림
            OnGameEnded = () => { isEventActive = false; };
        }

        Debug.Log("cctv 게임 시작");
    }
}
