using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 컨트롤 스크립트
/// </summary>

public class ButtonController : MonoBehaviour
{
    public CanvasGroup startButton;

    void Start()
    {
        StartCoroutine(BlinkButton());
    }

    // 컷씬
    public void StartButton(GameObject startPanel)
    {
        startPanel.SetActive(false);
    }

    public void OpeningStart(GameObject cutscene)
    {
        cutscene.SetActive(true);
    }

    // 버튼 점멸
    IEnumerator BlinkButton()
    {
        while (true)
        {
            for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * 2)
            {
                startButton.alpha = alpha;
                yield return null;
            }

            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * 2)
            {
                startButton.alpha = alpha;
                yield return null;
            }
        }
    }
}
