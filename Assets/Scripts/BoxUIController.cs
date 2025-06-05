using UnityEngine;
using UnityEngine.UI;

public class BoxUIController : MonoBehaviour
{
    public GameObject miniGamePanel;
    public GameObject resourceUIPanel;
    public ResourceUI resourceUI;

    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnClickOpenBox()
    {
        miniGamePanel.SetActive(true);    // �̴ϰ��� ����
        resourceUIPanel.SetActive(false);   // ���� UI�� ����

        // �̴ϰ��� ����, ���� �� OnMiniGameSuccess ȣ���
        BoxGameManager.Instance.StartMiniGame(OnMiniGameSuccess);
    }

    // �̴ϰ��� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    void OnMiniGameSuccess()
    {
        ResourceManager.Instance.OpenBox();  // ���� ���� (����)
        resourceUI.DisplayResource();            // ���� UI�� ��� ���
        resourceUIPanel.SetActive(true);       // ���� UI ǥ��
    }
}
