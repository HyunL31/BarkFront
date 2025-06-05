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

        // ���� ��쿡�� �̺�Ʈ Ÿ�̹� üũ
        if (isDay && eventManager != null)
        {
            eventManager.CheckEventTrigger(timer, dayDuration);
        }

        // ���� ����Ŭ�� �����ٸ� ��/�� ��ȯ
        if (timer >= currentCycle)
        {
            ToggleDayNight();
            timer = 0f;
        }
    }

    // ��/�� ���¸� ��ȯ�ϴ� �Լ�
    void ToggleDayNight()
    {
        isDay = !isDay;

        // �� ��ȯ
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
        // �� ��ȯ �� �ٽ� UI�� ����
        timerImage = GameObject.FindAnyObjectByType<Image>();
        dayNightText = GameObject.FindAnyObjectByType<TMP_Text>();
        eventManager = GameObject.FindAnyObjectByType<RobotEvent>();

        UpdateUI();
    }

    // ���� ��/�� ���¿� ���� UI �� �̺�Ʈ ���� �ʱ�ȭ
    void UpdateUI()
    {
        if (isDay)
        {
            dayNightText.text = "��";
            eventManager?.SetupRandomEvents(dayDuration);
        }
        else
        {
            dayNightText.text = "��";
            eventManager?.ResetEvents();
        }

        timerImage.fillAmount = 1f;
    }
}
