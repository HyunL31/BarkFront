using UnityEngine;

public class TimingBar : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform barArea;
    public RectTransform targetZone;

    public float speed = 200f;
    private bool goingRight = true;

    private int successCount = 0;
    public int maxStage = 3;
    public float targetShrinkFactor = 0.5f;

    void Update()
    {
        MovePointer(); // 매 프레임 포인터 이동

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPointerInTarget())
            {
                Debug.Log("성공");

                successCount++;

                if (successCount < maxStage)
                {
                    ShrinkTargetZone(); // 성공했으므로 영역을 줄임
                }
                else
                {
                    Debug.Log("최종 도달"); // 최대 단계 도달
                }
            }
            else
            {
                Debug.Log("실패"); // 정타를 벗어났다면 실패
            }
        }
    }

    // 포인터를 좌우로 이동시키는 함수
    void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;
        pointer.anchoredPosition += new Vector2(speed * direction * Time.deltaTime, 0f);

        // 오른쪽 끝에 도달하면 방향 반전
        if (pointer.anchoredPosition.x >= barArea.rect.width / 2)
        {
            goingRight = false;
        }

        // 왼쪽 끝에 도달하면 방향 반전
        else if (pointer.anchoredPosition.x <= -barArea.rect.width / 2)
        {
            goingRight = true;
        }
    }

    // 현재 포인터가 타겟 영역 안에 있는지 확인
    bool IsPointerInTarget()
    {
        float pointerX = pointer.anchoredPosition.x;
        float targetLeft = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetRight = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        return pointerX >= targetLeft && pointerX <= targetRight;
    }

    // 목표 영역의 너비를 줄이는 함수 (단계별 난이도 증가)
    void ShrinkTargetZone()
    {
        float newWidth = targetZone.rect.width * targetShrinkFactor;
        targetZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}
