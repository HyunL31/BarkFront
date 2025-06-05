using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 보상 타입 열거형 정의
public enum ResourceType { Snack, Oxygen, Coin }

// 간식 종류 열거형 정의
public enum SnackType { Bone, Chicken, Meat, Toy }

// 각 간식에 대응하는 강아지 클래스
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

    // 간식별 담당 강아지 리스트
    public List<DogCharacter> dogCharacters = new List<DogCharacter>();


    // 싱글톤 설정 및 씬 간 유지
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

    // 기본 캐릭터 이름 초기화 함수
    void InitializeDefaultDogs()
    {
        dogCharacters.Clear();
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Bone, dogName = "쮸리푸푸쮸팝차" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Chicken, dogName = "코냥찌피빠차찌" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Meat, dogName = "큥냥냥뿌야야유" });
        dogCharacters.Add(new DogCharacter { snackType = SnackType.Toy, dogName = "뽀쮸뽀쮸하뚜삐" });
    }

    // 특정 간식 타입의 캐릭터 이름을 수정하는 함수
    public void SetDogName(SnackType type, string newName)
    {
        var dog = dogCharacters.Find(d => d.snackType == type);

        if (dog != null)
        {
            dog.dogName = newName;
        }
    }

    // 특정 간식 타입의 캐릭터 이름을 반환하는 함수
    public string GetDogName(SnackType type)
    {
        var dog = dogCharacters.Find(d => d.snackType == type);
        return dog != null ? dog.dogName : "???";
    }

    // 보상 상자 열기 함수 (확률 기반)
    public void OpenBox()
    {
        float rand = Random.value * 100f; // 0~100 사이 난수

        // 0.5% 확률: 뼈
        if (rand < 0.5f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Bone;
            coinAmount = 0;
        }
        // 0.5% 확률: 치킨
        else if (rand < 1.0f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Chicken;
            coinAmount = 0;
        }
        // 0.5% 확률: 고기
        else if (rand < 1.5f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Meat;
            coinAmount = 0;
        }
        // 0.5% 확률: 장난감
        else if (rand < 2.0f)
        {
            currentResourceType = ResourceType.Snack;
            currentSnack = SnackType.Toy;
            coinAmount = 0;
        }
        // 0.5% 확률: 산소 아이템
        else if (rand < 2.5f)
        {
            currentResourceType = ResourceType.Oxygen;
            currentSnack = null;
            coinAmount = 0;
        }
        // 나머지 97.5% 확률: 코인 보상
        else
        {
            currentResourceType = ResourceType.Coin;
            currentSnack = null;

            // 1, 2, 3개 중 하나를 균등 확률(33.3%)로 선택
            int[] coinOptions = { 1, 2, 3 };
            coinAmount = coinOptions[Random.Range(0, 3)];
        }

        // 결과 출력 (디버깅용)
        Debug.Log($"보상 결과: {currentResourceType} - {currentSnack} / 코인: {coinAmount}");
    }
}
