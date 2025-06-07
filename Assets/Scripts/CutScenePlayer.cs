using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 컷씬 스크립트
/// </summary>

public class CutScenePlayer : MonoBehaviour
{
    // 인스펙터에서 설정 가능하도록 직렬화
    [SerializeField] private Image backgroundA;
    [SerializeField] private Image backgroundB;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> subtitles;

    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private float audioFadeDuration = 2f;

    public GameObject skipButton;

    public float cutsceneSpeed = 2.5f;
    public float subtitleSpeed = 0.05f;

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "GoodEnding")
        {
            StartCutscene();
        }
        else if (scene.name == "BadEnding")
        {
            StartCutscene();
        }
    }

    // Coroutine 시작
    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
        StartCoroutine(PlaySubtitle());

        skipButton.SetActive(true);
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

            // 알파값 조절 (배경 페이드)
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

            yield return new WaitForSeconds(cutsceneSpeed);
        }
    }

    // 자막 재생
    IEnumerator PlaySubtitle()
    {
        for (int i = 0; i < subtitles.Count; i++)
        {
            yield return StartCoroutine(ShowSubtitle(subtitles[i]));
        }

        yield return StartCoroutine(FadeOutAudio());

        yield return new WaitForSeconds(1f);

        if (SceneManager.GetActiveScene().name == "GoodEnding" || SceneManager.GetActiveScene().name == "BadEnding")
        {
            SceneManager.LoadScene("Opening");
        }
        else if (SceneManager.GetActiveScene().name == "Opening")
        {
            SceneManager.LoadScene("Day");
        }
    }

    // 자막 타이핑 효과 (코루틴)
    IEnumerator ShowSubtitle(string sentence)
    {
        subtitleText.text = "";

        foreach (char c in sentence)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(subtitleSpeed);
        }

        yield return new WaitForSeconds(1f);
    }

    // 배경음 페이드 아웃
    IEnumerator FadeOutAudio()
    {
        float startVolume = bgmAudio.volume;
        float t = 0f;

        while (t < audioFadeDuration)
        {
            t += Time.deltaTime;
            bgmAudio.volume = Mathf.Lerp(startVolume, 0f, t / audioFadeDuration);
            yield return null;
        }

        bgmAudio.volume = 0f;
        bgmAudio.Stop();
    }
}
