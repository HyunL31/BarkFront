using UnityEngine;
using TMPro;
public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;
    void Start()
    {
        UpdateCoinUI(ResourceManager.Instance.GetCoins());
        ResourceManager.Instance.OnCoinChanged += UpdateCoinUI;
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnCoinChanged -= UpdateCoinUI;
        }
    }

    void UpdateCoinUI(int amount)
    {
        coinText.text = $"{amount}";
    }

}
