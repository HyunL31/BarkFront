using UnityEngine;

public class LootResult : MonoBehaviour
{
    public UIManager uiManager;

    public int currentDay = 1;

    public void GiveLoot()
    {
        // 미니게임 시작
        BoxGameManager.Instance.StartMiniGame(OnMiniGameSuccess);
    }

    void OnMiniGameSuccess()
    {
        Debug.Log("🎯 OnMiniGameSuccess 실행됨");

        if (DayNightTimer.Instance == null)
        {
            Debug.LogWarning("❗ DayNightTimer.Instance == null");
            return;
        }

        int day = DayNightTimer.Instance.GetCurrentDay();

        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("❗ ResourceManager.Instance == null");
            return;
        }

        ResourceManager.Instance.OpenBox(day);

        uiManager.ShowResourceUI();
    }
}
