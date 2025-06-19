using UnityEngine;
using PixelBattleText;

public class TestingPixelBattleText : MonoBehaviour
{
    public TextAnimation textAnimation;

    void Start()
    {
        PixelBattleTextController.DisplayText("Night falls!", textAnimation, Vector3.one * 0.5f);
    }
}