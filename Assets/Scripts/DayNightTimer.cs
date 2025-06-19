using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayNightTimer : MonoBehaviour
{
    public float dayDuration = 100f;
    private float timer = 0f;
    private bool isDay = true;
    public int currentDay = 1;

    public Image timerImage;
    public TMP_Text dayNightText;
    public RobotEvent eventManager;

    private static DayNightTimer instance;
    public static DayNightTimer Instance => instance;

    private bool badEndingTriggered = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (isDay)
        {
            timer += Time.deltaTime;

            if (timerImage != null)
                timerImage.fillAmount = 1f - (timer / dayDuration);

            if (eventManager != null)
                eventManager.CheckEventTrigger(timer, dayDuration); // ³· ÀÌº¥Æ® À¯Áö

            if (timer >= dayDuration)
            {
                timer = 0f;
                GoToNight();
            }
        }
        else
        {
            // ¹ã: ·Îº¿ Àü¸êÇÏ¸é ´ÙÀ½ ³¯·Î ÀüÈ¯
            if (AreAllRobotsDefeated())
            {
                GoToDay();
            }
        }
    }

    void GoToDay()
    {
        currentDay++;

        if (currentDay > 3 && !badEndingTriggered)
        {
            SceneManager.LoadScene("GoodEnding");
        }
        else
        {
            isDay = true;
            SceneManager.LoadScene("Day");
        }
    }

    void GoToNight()
    {
        isDay = false;
        SceneManager.LoadScene("Night");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timerImage = GameObject.Find("TimerCircle")?.GetComponent<Image>();
        dayNightText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();
        eventManager = GameObject.FindFirstObjectByType<RobotEvent>(FindObjectsInactive.Include);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (dayNightText != null && isDay)
        {
            dayNightText.text = $"Day {currentDay}";
        }

        if (isDay)
            eventManager?.SetupRandomEvents(dayDuration);
        else
            eventManager?.ResetEvents();

        if (timerImage != null)
            timerImage.fillAmount = 1f;
    }

    public int GetCurrentDay() => currentDay;

    bool AreAllRobotsDefeated()
    {
        return GameObject.FindObjectsByType<RobotController>(FindObjectsSortMode.None).Length == 0;
    }

    public void SetBadEndingTriggered()
    {
        badEndingTriggered = true;
    }
}