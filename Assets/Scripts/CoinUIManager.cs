using UnityEngine;
using TMPro;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;
    public int currentCoins = 120; 

    void Start()
    {
        UpdateCoinUI();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;
        if (currentCoins < 0) currentCoins = 0;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        coinText.text = $"{currentCoins}";
    }
}
