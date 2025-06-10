using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// 낮, 밤 전환용 타이머 스크립트
/// </summary>

public class DayNightTimer : MonoBehaviour
{
    public float dayDuration = 100f;
    public float nightDuration = 600f;

    private float timer = 0f;
    private bool isDay = true;

    public int currentDay = 1;

    public Image timerImage;
    public TMP_Text dayNightText;

    public RobotEvent eventManager;

    private static DayNightTimer instance;
    public static DayNightTimer Instance => instance;

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
        if (timerImage != null)
            timerImage.fillAmount = 1f - (timer / currentCycle);

        if (isDay && eventManager != null)
        {
            eventManager.CheckEventTrigger(timer, dayDuration);
        }

        if (timer >= currentCycle)
        {
            ToggleDayNight();
            timer = 0f;
        }
    }

    void ToggleDayNight()
    {
        isDay = !isDay;

        if (isDay)
        {
            currentDay++;
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
        timerImage = GameObject.FindAnyObjectByType<Image>();
        dayNightText = GameObject.FindAnyObjectByType<TMP_Text>();
        eventManager = GameObject.FindAnyObjectByType<RobotEvent>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (dayNightText != null)
        {
            dayNightText.text = isDay ? $"Day {currentDay}" : $"Day {currentDay}";
        }

        if (isDay)
        {
            eventManager?.SetupRandomEvents(dayDuration);
        }
        else
        {
            eventManager?.ResetEvents();
        }

        if (timerImage != null)
        {
            timerImage.fillAmount = 1f;
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }
}