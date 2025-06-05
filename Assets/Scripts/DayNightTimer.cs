using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DayNightTimer : MonoBehaviour
{
    public float dayDuration = 600f;
    public float nightDuration = 600f;

    private float timer = 0f;
    private bool isDay = true;

    public Image timerImage;
    public TMP_Text dayNightText;

    public RobotEvent eventManager;

    private static DayNightTimer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float currentCycle = isDay ? dayDuration : nightDuration;

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

        // 씬 전환
        if (isDay)
        {
            SceneManager.LoadScene("Day");
        }
        else
        {
            SceneManager.LoadScene("Night");
        }

        UpdateUI();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 후 다시 UI와 연결
        timerImage = GameObject.FindAnyObjectByType<Image>();
        dayNightText = GameObject.FindAnyObjectByType<TMP_Text>();
        eventManager = GameObject.FindAnyObjectByType<RobotEvent>();

        UpdateUI();
    }

    // 현재 낮/밤 상태에 맞춰 UI 및 이벤트 상태 초기화
    void UpdateUI()
    {
        if (isDay)
        {
            dayNightText.text = "낮";
            eventManager?.SetupRandomEvents(dayDuration);
        }
        else
        {
            dayNightText.text = "밤";
            eventManager?.ResetEvents();
        }

        timerImage.fillAmount = 1f;
    }
}
