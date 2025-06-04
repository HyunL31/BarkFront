using UnityEngine;
using System;

public class BoxGameManager : MonoBehaviour
{
    public static BoxGameManager Instance;

    private Action onSuccessCallback;            // 미니게임 성공 시 실행할 콜백 함수

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // 미니게임 시작 요청 시 호출됨
    // 콜백을 등록해두고, 성공 시 호출되도록 설정
    public void StartMiniGame(Action onSuccess)
    {
        this.onSuccessCallback = onSuccess;
    }

    // 미니게임이 성공했을 때 호출되는 함수
    public void MiniGameSuccess()
    {
        Debug.Log("미니게임 성공");

        // 등록된 콜백 실행
        onSuccessCallback?.Invoke();

        // 미니게임 비활성화
        gameObject.SetActive(false);
    }

    // 실패 처리 예정
}
