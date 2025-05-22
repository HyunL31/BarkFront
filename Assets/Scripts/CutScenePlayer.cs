using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlayer : MonoBehaviour
{
    // 인스펙터에서 설정 가능하도록 직렬화
    [SerializeField] private Image backgroundA;
    [SerializeField] private Image backgroundB;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> subtitles;

    // Coroutine 시작
    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
        StartCoroutine(PlaySubtitle());
    }

    // 배경 재생 (코루틴)
    IEnumerator PlayCutscene()
    {
        backgroundA.sprite = backgrounds[0];
        backgroundA.color = new Color(0, 0, 0, 1);
        backgroundB.color = new Color(1, 1, 1, 0);

        for (int i = 1; i < backgrounds.Count; i++)
        {
            // 다음 배경 설정
            backgroundB.sprite = backgrounds[i];

            // 알파값 서서히 조절 (배경 페이드)
            float t = 0;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = t / fadeDuration;

                backgroundA.color = new Color(1, 1, 1, 1 - alpha);
                backgroundB.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            // A와 B Swap
            var temp = backgroundA;
            backgroundA = backgroundB;
            backgroundB = temp;

            backgroundB.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(2.5f); // 배경 유지 시간
        }
    }

    // 자막 재생 (코루틴)
    IEnumerator PlaySubtitle()
    {
        for (int i = 0; i < subtitles.Count; i++)
        {
            yield return StartCoroutine(ShowSubtitle(subtitles[i]));
        }
    }

    // 자막 타이핑 효과 (코루틴)
    IEnumerator ShowSubtitle(string sentence)
    {
        subtitleText.text = "";

        foreach (char c in sentence)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(0.05f); // 속도 조절 가능
        }

        yield return new WaitForSeconds(1f); // 자막 유지 시간
    }
}
