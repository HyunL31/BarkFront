using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField] private Image backgroundA;
    [SerializeField] private Image backgroundB;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> subtitles;

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        backgroundA.sprite = backgrounds[0];
        backgroundA.color = Color.white;
        backgroundB.color = new Color(1, 1, 1, 0); // 투명

        subtitleText.text = "";
        yield return StartCoroutine(ShowSubtitle(subtitles[0]));

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

            // A와 B 스왑
            var temp = backgroundA;
            backgroundA = backgroundB;
            backgroundB = temp;

            backgroundB.color = new Color(1, 1, 1, 0); // 다시 투명

            // 자막 갱신
            subtitleText.text = "";
            yield return StartCoroutine(ShowSubtitle(subtitles[i]));
        }

        // 컷씬 끝나면 다음 씬으로 넘어가기 등
    }

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
