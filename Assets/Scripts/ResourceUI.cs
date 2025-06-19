using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [Header("UI 연결")]
    public TMP_Text resourceText;
    public Image resourceImage;

    [Header("스프라이트")]
    public Sprite boneSprite;
    public Sprite chickenSprite;
    public Sprite meatSprite;
    public Sprite toySprite;
    public Sprite oxygenSprite;
    public Sprite coinSprite;

    public void DisplayResource()
    {
        var resource = ResourceManager.Instance;

        switch (resource.currentResourceType)
        {
            //case ResourceType.Snack:
            //    if (resource.currentSnack.HasValue)
            //    {
            //        SnackType snack = resource.currentSnack.Value;
            //        string dogName = resource.GetDogName(snack);
            //        resourceText.text = $"{dogName}의 간식: {snack}";
            //        resourceImage.sprite = GetSnackSprite(snack);
            //        Debug.Log($"🍖 [ResourceUI] 간식: {snack}, 이름: {dogName}");
            //    }
            //    break;

            case ResourceType.Oxygen:
                resourceText.text = "산소 아이템 획득!";
                resourceImage.sprite = oxygenSprite;
                break;

            case ResourceType.Coin:
                resourceText.text = $"코인 {resource.coinAmount}개 획득!";
                resourceImage.sprite = coinSprite;
                break;

            default:
                resourceText.text = "알 수 없는 보상";
                resourceImage.sprite = null;
                break;
        }
    }

    private Sprite GetSnackSprite(SnackType type)
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