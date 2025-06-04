using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightTimer : MonoBehaviour
{
    public float dayDuration = 600f;
    public float nightDuration = 600f;

    private float timer = 0f;
    private bool isDay = true;

    public Image timerImage;
    public TMP_Text dayNightText;

    public RobotEvent eventManager;

    void Start()
    {
        UpdateUI(); // 시작할 때 UI 초기화
    }

    void Update()
    {
        timer += Time.deltaTime; // 매 프레임 시간 누적

        float currentCycle = isDay ? dayDuration : nightDuration;

        // 진행률에 따라 타이머 이미지 업데이트 (fillAmount는 1~0 사이)
        timerImage.fillAmount = 1f - (timer / currentCycle);

        // 낮인 경우에만 이벤트 타이밍 체크
        if (isDay && eventManager != null)
        {
            eventManager.CheckEventTrigger(timer, dayDuration);
        }

        // 현재 사이클이 끝났다면 낮/밤 전환
        if (timer >= currentCycle)
        {
            ToggleDayNight();
            timer = 0f;
        }
    }

    // 낮/밤 상태를 전환하는 함수
    void ToggleDayNight()
    {
        isDay = !isDay;
        UpdateUI(); // 전환 후 UI 및 이벤트 초기화
    }

    // 현재 낮/밤 상태에 맞춰 UI 및 이벤트 상태 초기화
    void UpdateUI()
    {
        if (isDay)
        {
            dayNightText.text = "낮"; // UI 텍스트 설정
            eventManager?.SetupRandomEvents(dayDuration); // 낮 이벤트 설정
        }
        else
        {
            dayNightText.text = "밤"; // UI 텍스트 설정
            eventManager?.ResetEvents(); // 밤에는 이벤트 리셋
        }

        timerImage.fillAmount = 1f; // 진행 바 초기화
    }
}
