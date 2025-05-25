using UnityEngine;

public class TimingBar : MonoBehaviour
{
    public RectTransform pointer; // 움직이는 포인터
    public RectTransform barArea; // 전체 바
    public RectTransform targetZone; // 성공 영역

    public float speed = 200f; // 포인터 이동 속도
    private bool goingRight = true;

    void Update()
    {
        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPointerInTarget())
            {
                Debug.Log("성공!");
            }
            else
            {
                Debug.Log("실패...");
            }
        }
    }

    void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;
        pointer.anchoredPosition += new Vector2(speed * direction * Time.deltaTime, 0f);

        if (pointer.anchoredPosition.x >= barArea.rect.width / 2)
            goingRight = false;
        else if (pointer.anchoredPosition.x <= -barArea.rect.width / 2)
            goingRight = true;
    }

    bool IsPointerInTarget()
    {
        float pointerX = pointer.anchoredPosition.x;
        float targetLeft = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetRight = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        return pointerX >= targetLeft && pointerX <= targetRight;
    }
}
