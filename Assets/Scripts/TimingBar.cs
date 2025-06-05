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
        MovePointer(); // �� ������ ������ �̵�

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPointerInTarget())
            {
                Debug.Log("����");

                successCount++;

                if (successCount < maxStage)
                {
                    ShrinkTargetZone(); // ���������Ƿ� ������ ����
                }
                else
                {
                    Debug.Log("���� ����"); // �ִ� �ܰ� ����
                }
            }
            else
            {
                Debug.Log("����"); // ��Ÿ�� ����ٸ� ����
            }
        }
    }

    // �����͸� �¿�� �̵���Ű�� �Լ�
    void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;
        pointer.anchoredPosition += new Vector2(speed * direction * Time.deltaTime, 0f);

        // ������ ���� �����ϸ� ���� ����
        if (pointer.anchoredPosition.x >= barArea.rect.width / 2)
        {
            goingRight = false;
        }

        // ���� ���� �����ϸ� ���� ����
        else if (pointer.anchoredPosition.x <= -barArea.rect.width / 2)
        {
            goingRight = true;
        }
    }

    // ���� �����Ͱ� Ÿ�� ���� �ȿ� �ִ��� Ȯ��
    bool IsPointerInTarget()
    {
        float pointerX = pointer.anchoredPosition.x;
        float targetLeft = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetRight = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        return pointerX >= targetLeft && pointerX <= targetRight;
    }

    // ��ǥ ������ �ʺ� ���̴� �Լ� (�ܰ躰 ���̵� ����)
    void ShrinkTargetZone()
    {
        float newWidth = targetZone.rect.width * targetShrinkFactor;
        targetZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}
