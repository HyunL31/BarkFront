using UnityEngine;
using System;
using System.Collections;

public class BoxGameManager : MonoBehaviour
{
    public static BoxGameManager Instance;
    public TimingBar timingBar;

    public AudioSource fail;
    public GameObject failPanel;

    private Action onSuccessCallback;

    public bool IsPlayingMiniGame => timingBar.gameObject.activeSelf;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameObject.SetActive(true);
        timingBar.gameObject.SetActive(false);
    }

    public void StartMiniGame(Action onSuccess)
    {
        onSuccessCallback = onSuccess;
        gameObject.SetActive(true);
        timingBar.ResetBar(this);
    }

    public void OnTimingSuccess()
    {
        if (onSuccessCallback != null)
        {
            onSuccessCallback.Invoke();
        }

        gameObject.SetActive(false);
    }

    public void OnTimingFail()
    {
        StartCoroutine(HandleFailThenDisable());
    }

    IEnumerator HandleFailThenDisable()
    {
        if (failPanel != null)
        {
            failPanel.SetActive(true);
            fail.Play();
        }

        yield return new WaitForSeconds(1.5f);

        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    // For CCTV Game
    public void ForceClose()
    {
        gameObject.SetActive(false);
        timingBar.gameObject.SetActive(false);
    }
}