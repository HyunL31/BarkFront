using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text resourceText;
    public Image resourceImage;

    // ������ ���� �����ۿ� �����Ǵ� ��������Ʈ �̹���
    public Sprite boneSprite;
    public Sprite chickenSprite;
    public Sprite meatSprite;
    public Sprite toySprite;
    public Sprite oxygenSprite;
    public Sprite coinSprite;

    // ResourceManager���� ���� ������ �޾ƿ� UI�� ǥ���ϴ� �Լ�
    public void DisplayResource()
    {
        var resource = ResourceManager.Instance;
        string dogName = ResourceManager.Instance.GetDogName(resource.currentSnack.Value);

        switch (resource.currentResourceType)
        {
            case ResourceType.Snack:
                resourceText.text = $"{dogName}�� ����: {resource.currentSnack}";
                resourceImage.sprite = GetSnackSprite(resource.currentSnack.Value);
                break;

            case ResourceType.Oxygen:
                resourceText.text = "��� ������ ȹ��!";
                resourceImage.sprite = oxygenSprite;
                break;

            case ResourceType.Coin:
                resourceText.text = $"���� {resource.coinAmount}�� ȹ��!";
                resourceImage.sprite = coinSprite;
                break;
        }
    }

    // ���� Ÿ�Կ� ���� ������ ��������Ʈ ��ȯ
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
