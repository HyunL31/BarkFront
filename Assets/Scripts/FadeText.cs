using System.Collections;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    public GameObject info;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = info.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = info.AddComponent<CanvasGroup>();
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(1f);

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
