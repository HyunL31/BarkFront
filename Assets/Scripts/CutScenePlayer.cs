using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �ƾ� ��ũ��Ʈ
/// </summary>

public class CutScenePlayer : MonoBehaviour
{
    // �ν����Ϳ��� ���� �����ϵ��� ����ȭ
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

    // Coroutine ����
    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
        StartCoroutine(PlaySubtitle());

        skipButton.SetActive(true);
    }

    // ��� ��� (�ڷ�ƾ)
    IEnumerator PlayCutscene()
    {
        backgroundA.sprite = backgrounds[0];
        backgroundA.color = new Color(0, 0, 0, 1);
        backgroundB.color = new Color(1, 1, 1, 0);

        for (int i = 1; i < backgrounds.Count; i++)
        {
            // ���� ��� ����
            backgroundB.sprite = backgrounds[i];

            // ���İ� ���� (��� ���̵�)
            float t = 0;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = t / fadeDuration;

                backgroundA.color = new Color(1, 1, 1, 1 - alpha);
                backgroundB.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            // A�� B Swap
            var temp = backgroundA;
            backgroundA = backgroundB;
            backgroundB = temp;

            backgroundB.color = new Color(1, 1, 1, 0);

            yield return new WaitForSeconds(cutsceneSpeed);
        }
    }

    // �ڸ� ���
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

    // �ڸ� Ÿ���� ȿ�� (�ڷ�ƾ)
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

    // ����� ���̵� �ƿ�
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
