using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public CanvasGroup startButton;

    void Start()
    {
        StartCoroutine(BlinkButton());
    }

    public void StartButton(GameObject startPanel)
    {
        startPanel.SetActive(false);
    }

    public void OpeningStart(GameObject cutscene)
    {
        cutscene.SetActive(true);
    }

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
