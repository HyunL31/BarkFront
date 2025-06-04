using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� Ÿ�� ������ ����
public enum ResourceType { Snack, Oxygen, Coin }

// ���� ���� ������ ����
public enum SnackType { Bone, Chicken, Meat, Toy }

// �� ���Ŀ� �����ϴ� ������ Ŭ����
[System.Serializable]
public class DogCharacter
{
    public SnackType snackType;
    public string dogName;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public ResourceType currentResourceType;
    public SnackType? currentSnack;
    public int coinAmount = 0;

    // ���ĺ� ��� ������ ����Ʈ
    public List<DogCharacter> dogCharacters = new List<DogCharacter>();


    // �̱��� ���� �� �� �� ����
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDefaultDogs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �⺻ ĳ���� �̸� �ʱ�ȭ �Լ�
    void InitializeDefaultDogs()
    {
        dogCharacters.Clear();
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Bone, dogName = "�鸮ǪǪ������" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Chicken, dogName = "�ڳ����Ǻ�����" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Meat, dogName = "�r�ɳɻѾ߾���" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Toy, dogName = "��������϶ѻ�" });
    }

    // Ư�� ���� Ÿ���� ĳ���� �̸��� �����ϴ� �Լ�
    public void SetDogName(SnackType type, string newName)
    {
        var dog = dogCharacters.Find(d => d.snackType == type);

        if (dog != null)
        {
            dog.dogName = newName;
        }
    }

    // Ư�� ���� Ÿ���� ĳ���� �̸��� ��ȯ�ϴ� �Լ�
    public string GetDogName(SnackType type)
    {
        var dog = dogCharacters.Find(d => d.snackType == type);
        return dog != null ? dog.dogName : "???";
    }

    // ���� ���� ���� �Լ� (Ȯ�� ���)
    public void OpenBox()
    {
        float rand = Random.value * 100f; // 0~100 ���� ����

        // 0.5% Ȯ��: ��
        if (rand < 0.5f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Bone;
            coinAmount = 0;
        }
        // 0.5% Ȯ��: ġŲ
        else if (rand < 1.0f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Chicken;
            coinAmount = 0;
        }
        // 0.5% Ȯ��: ���
        else if (rand < 1.5f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Meat;
            coinAmount = 0;
        }
        // 0.5% Ȯ��: �峭��
        else if (rand < 2.0f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Toy;
            coinAmount = 0;
        }
        // 0.5% Ȯ��: ��� ������
        else if (rand < 2.5f)
        {
            currentResourceType = ResourceType.Oxygen;
            currentSnack = null;
            coinAmount = 0;
        }
        // ������ 97.5% Ȯ��: ���� ����
        else
        {
            currentResourceType = ResourceType.Coin;
            currentSnack = null;

            // 1, 2, 3�� �� �ϳ��� �յ� Ȯ��(33.3%)�� ����
            int[] coinOptions = { 1, 2, 3 };
            coinAmount = coinOptions[Random.Range(0, 3)];
        }

        // ��� ��� (������)
        Debug.Log($"���� ���: {currentResourceType} - {currentSnack} / ����: {coinAmount}");
    }
}
