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
    public float targetShrinkFactor = 0.1f;

    private BoxGameManager manager;

    void Update()
    {
        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPointerInTarget())
            {
                successCount++;

                if (successCount < maxStage)
                {
                    ShrinkTargetZone();
                }
                else
                {
                    manager.OnTimingSuccess();
                }
            }
            else
            {
                manager.OnTimingFail();
            }
        }
    }

    public void ResetBar(BoxGameManager mgr)
    {
        successCount = 0;
        targetZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
        manager = mgr;
        pointer.anchoredPosition = Vector2.zero;
        goingRight = true;
    }

    void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;
        pointer.anchoredPosition += new Vector2(speed * direction * Time.deltaTime, 0f);

        if (pointer.anchoredPosition.x >= barArea.rect.width / 2 - 100)
        {
            goingRight = false;
        }

        else if (pointer.anchoredPosition.x <= -(barArea.rect.width / 2 - 100))
        {
            goingRight = true;
        }
    }

    bool IsPointerInTarget()
    {
        float pointerX = pointer.anchoredPosition.x;
        float targetLeft = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetRight = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        return pointerX >= targetLeft && pointerX <= targetRight;
    }

    // Width of TargetZone
    void ShrinkTargetZone()
    {
        float newWidth = targetZone.rect.width * targetShrinkFactor;
        targetZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}
