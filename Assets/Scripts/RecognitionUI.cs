using UnityEngine;
using TMPro;

public class RecognitionUI : MonoBehaviour
{
    public TextMeshProUGUI resultTextUI;
    public void UpdateUI(string recognizedText, float accuracy, bool isCorrect)
    {
        string result = $"Recognized: {recognizedText}\nAccuracy: {accuracy:F2}";
        if (isCorrect)
            result += "\nCorrect! Prefab will be summoned.";
        else
            result += "\nLow accuracy. Try again.";

        Debug.Log("UI 업데이트 호출됨: " + result); // ✅ 꼭 추가!
        resultTextUI.text = result;
    }

}

