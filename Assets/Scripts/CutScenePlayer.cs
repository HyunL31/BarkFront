using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutScenePlayer : MonoBehaviour
{
    [SerializeField] private Image backgroundA;
    [SerializeField] private Image backgroundB;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> subtitles;

    private bool isCutsceneDone = false;
    private bool isSubtitleDone = false;

    public void StartCutscene()
    {
        StartCoroutine(PlayFullCutscene());
    }

    IEnumerator PlayFullCutscene()
    {
        // 자막 초기화
        subtitleText.color = new Color(1, 1, 1, 1);
        subtitleText.text = "";

        // 두 작업을 동시에 시작
        StartCoroutine(PlayCutscene());
        StartCoroutine(PlaySubtitle());

        // 둘 다 끝날 때까지 대기
        while (!isCutsceneDone || !isSubtitleDone)
        {
            yield return null;
        }

        SceneManager.LoadScene("Day");
    }

    IEnumerator PlayCutscene()
    {
        backgroundA.sprite = backgrounds[0];
        backgroundA.color = new Color(0, 0, 0, 1);
        backgroundB.color = new Color(1, 1, 1, 0);

        for (int i = 1; i < backgrounds.Count; i++)
        {
            backgroundB.sprite = backgrounds[i];
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = t / fadeDuration;

                backgroundA.color = new Color(1, 1, 1, 1 - alpha);
                backgroundB.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            var temp = backgroundA;
            backgroundA = backgroundB;
            backgroundB = temp;

            backgroundB.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(2.5f);
        }

        isCutsceneDone = true; // ✅ 컷씬 끝남
    }

    IEnumerator PlaySubtitle()
    {
        for (int i = 0; i < subtitles.Count; i++)
        {
            yield return StartCoroutine(ShowSubtitle(subtitles[i]));
        }

        isSubtitleDone = true; // ✅ 자막 끝남
    }

    IEnumerator ShowSubtitle(string sentence)
    {
        subtitleText.text = "";

        foreach (char c in sentence)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
    }
}
