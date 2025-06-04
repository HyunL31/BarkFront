using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text resourceText;
    public Image resourceImage;

    // 각각의 보상 아이템에 대응되는 스프라이트 이미지
    public Sprite boneSprite;
    public Sprite chickenSprite;
    public Sprite meatSprite;
    public Sprite toySprite;
    public Sprite oxygenSprite;
    public Sprite coinSprite;

    // ResourceManager에서 보상 정보를 받아와 UI에 표시하는 함수
    public void DisplayResource()
    {
        var resource = ResourceManager.Instance;
        string dogName = ResourceManager.Instance.GetDogName(resource.currentSnack.Value);

        switch (resource.currentResourceType)
        {
            case ResourceType.Snack:
                resourceText.text = $"{dogName}의 간식: {resource.currentSnack}";
                resourceImage.sprite = GetSnackSprite(resource.currentSnack.Value);
                break;

            case ResourceType.Oxygen:
                resourceText.text = "산소 아이템 획득!";
                resourceImage.sprite = oxygenSprite;
                break;

            case ResourceType.Coin:
                resourceText.text = $"코인 {resource.coinAmount}개 획득!";
                resourceImage.sprite = coinSprite;
                break;
        }
    }

    // 간식 타입에 따라 적절한 스프라이트 반환
    Sprite GetSnackSprite(SnackType type)
    {
        return type switch
        {
            SnackType.Bone => boneSprite,
            SnackType.Chicken => chickenSprite,
            SnackType.Meat => meatSprite,
            SnackType.Toy => toySprite,
            _ => null
        };
    }
}
