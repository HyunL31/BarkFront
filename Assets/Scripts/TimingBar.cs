using UnityEngine;

public class TimingBar : MonoBehaviour
{
    public RectTransform pointer; // �����̴� ������
    public RectTransform barArea; // ��ü ��
    public RectTransform targetZone; // ���� ����

    public float speed = 200f; // ������ �̵� �ӵ�
    private bool goingRight = true;

    void Update()
    {
        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPointerInTarget())
            {
                Debug.Log("����!");
            }
            else
            {
                Debug.Log("����...");
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
