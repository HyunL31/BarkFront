using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlayer : MonoBehaviour
{
    // �ν����Ϳ��� ���� �����ϵ��� ����ȭ
    [SerializeField] private Image backgroundA;
    [SerializeField] private Image backgroundB;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<string> subtitles;

    // Coroutine ����
    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
        StartCoroutine(PlaySubtitle());
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

            // ���İ� ������ ���� (��� ���̵�)
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

            yield return new WaitForSeconds(2.5f); // ��� ���� �ð�
        }
    }

    // �ڸ� ��� (�ڷ�ƾ)
    IEnumerator PlaySubtitle()
    {
        for (int i = 0; i < subtitles.Count; i++)
        {
            yield return StartCoroutine(ShowSubtitle(subtitles[i]));
        }
    }

    // �ڸ� Ÿ���� ȿ�� (�ڷ�ƾ)
    IEnumerator ShowSubtitle(string sentence)
    {
        subtitleText.text = "";

        foreach (char c in sentence)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(0.05f); // �ӵ� ���� ����
        }

        yield return new WaitForSeconds(1f); // �ڸ� ���� �ð�
    }
}
