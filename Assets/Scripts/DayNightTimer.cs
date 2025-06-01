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
        UpdateUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float currentCycle = isDay ? dayDuration : nightDuration;
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
        UpdateUI();
    }

    void UpdateUI()
    {
        if (isDay)
        {
            dayNightText.text = "³·";
            eventManager?.SetupRandomEvents(dayDuration);
        }
        else
        {
            dayNightText.text = "¹ã";
            eventManager?.ResetEvents();
        }

        timerImage.fillAmount = 1f;
    }
}
