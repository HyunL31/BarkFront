using UnityEngine;
using System;

public class BoxGameManager : MonoBehaviour
{
    public static BoxGameManager Instance;
    public TimingBar timingBar;

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
        gameObject.SetActive(false);
    }

    // For CCTV Game
    public void ForceClose()
    {
        gameObject.SetActive(false);
        timingBar.gameObject.SetActive(false);
    }
}