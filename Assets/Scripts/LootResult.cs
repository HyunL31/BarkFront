using UnityEngine;

public class LootResult : MonoBehaviour
{
    public int currentDay = 1;

    public void GiveLoot()
    {
        // 미니게임 시작
        BoxGameManager.Instance.StartMiniGame(OnMiniGameSuccess);
    }

    void OnMiniGameSuccess()
    {
        if (DayNightTimer.Instance == null)
        {
            return;
        }

        int day = DayNightTimer.Instance.GetCurrentDay();

        if (ResourceManager.Instance == null)
        {
            return;
        }

        ResourceManager.Instance.OpenBox(day);

        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.ShowResourceUI();
    }
}
