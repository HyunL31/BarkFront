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
        UpdateUI(); // ������ �� UI �ʱ�ȭ
    }

    void Update()
    {
        timer += Time.deltaTime; // �� ������ �ð� ����

        float currentCycle = isDay ? dayDuration : nightDuration;

        // ������� ���� Ÿ�̸� �̹��� ������Ʈ (fillAmount�� 1~0 ����)
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
        UpdateUI(); // ��ȯ �� UI �� �̺�Ʈ �ʱ�ȭ
    }

    // ���� ��/�� ���¿� ���� UI �� �̺�Ʈ ���� �ʱ�ȭ
    void UpdateUI()
    {
        if (isDay)
        {
            dayNightText.text = "��"; // UI �ؽ�Ʈ ����
            eventManager?.SetupRandomEvents(dayDuration); // �� �̺�Ʈ ����
        }
        else
        {
            dayNightText.text = "��"; // UI �ؽ�Ʈ ����
            eventManager?.ResetEvents(); // �㿡�� �̺�Ʈ ����
        }

        timerImage.fillAmount = 1f; // ���� �� �ʱ�ȭ
    }
}
