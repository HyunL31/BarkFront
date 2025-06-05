using UnityEngine;
using UnityEngine.UI;

public class BoxUIController : MonoBehaviour
{
    public GameObject miniGamePanel;
    public GameObject resourceUIPanel;
    public ResourceUI resourceUI;

    // 상자 열기 버튼 클릭 시 호출되는 함수
    public void OnClickOpenBox()
    {
        miniGamePanel.SetActive(true);    // 미니게임 시작
        resourceUIPanel.SetActive(false);   // 보상 UI는 숨김

        // 미니게임 시작, 성공 시 OnMiniGameSuccess 호출됨
        BoxGameManager.Instance.StartMiniGame(OnMiniGameSuccess);
    }

    // 미니게임 성공 시 호출되는 콜백 함수
    void OnMiniGameSuccess()
    {
        ResourceManager.Instance.OpenBox();  // 보상 결정 (랜덤)
        resourceUI.DisplayResource();            // 보상 UI에 결과 출력
        resourceUIPanel.SetActive(true);       // 보상 UI 표시
    }
}
