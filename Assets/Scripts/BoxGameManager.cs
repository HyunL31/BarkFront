using UnityEngine;
using System;

public class BoxGameManager : MonoBehaviour
{
    public static BoxGameManager Instance;

    private Action onSuccessCallback;            // �̴ϰ��� ���� �� ������ �ݹ� �Լ�

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // �̴ϰ��� ���� ��û �� ȣ���
    // �ݹ��� ����صΰ�, ���� �� ȣ��ǵ��� ����
    public void StartMiniGame(Action onSuccess)
    {
        this.onSuccessCallback = onSuccess;
    }

    // �̴ϰ����� �������� �� ȣ��Ǵ� �Լ�
    public void MiniGameSuccess()
    {
        Debug.Log("�̴ϰ��� ����");

        // ��ϵ� �ݹ� ����
        onSuccessCallback?.Invoke();

        // �̴ϰ��� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    // ���� ó�� ����
}
