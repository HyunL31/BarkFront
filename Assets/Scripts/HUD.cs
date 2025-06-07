using UnityEngine;
using TMPro;

/// <summary>
/// 자원 HUD 스크립트
/// </summary>

public class HUD : MonoBehaviour
{
    public bool isRewardShowing;

    [Header("자원 종류")]
    public TMP_Text oxygenText;
    public TMP_Text coinText;
    public TMP_Text toyText;
    public TMP_Text boneText;
    public TMP_Text chickenText;
    public TMP_Text meatText;

    // 자원 업데이트
    void Update()
    {
        if (isRewardShowing)
        {
            return;
        }

        var rm = ResourceManager.Instance;

        coinText.text = ResourceManager.Instance.totalCoins.ToString();
        oxygenText.text = $"{ResourceManager.Instance.oxygenPercent}%";

        //boneText.text = GetSnackCount(SnackType.Bone);
        //chickenText.text = GetSnackCount(SnackType.Chicken);
        //meatText.text = GetSnackCount(SnackType.Meat);
        //toyText.text = GetSnackCount(SnackType.Toy);
    }

    //string GetSnackCount(SnackType type)
    //{
    //    var rm = ResourceManager.Instance;
    //    return rm.snackInventory.TryGetValue(type, out int count) ? count.ToString() : "0";
    //}
}