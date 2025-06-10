using System.Collections.Generic;
using UnityEngine;

public enum ResourceType { Snack, Oxygen, Coin }
public enum SnackType { Bone, Chicken, Meat, Toy }

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
    public int oxygenPercent = 0;

    public int totalCoins = 0;

    //public Dictionary<SnackType, int> snackInventory = new();
    public List<DogCharacter> dogCharacters = new();
    public event System.Action<int> OnCoinChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //InitializeDefaultDogs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //void InitializeDefaultDogs()
    //{
    //    dogCharacters.Clear();
    //    dogCharacters.Add(new DogCharacter { snackType = SnackType.Bone, dogName = "AjouUniversity" });
    //    dogCharacters.Add(new DogCharacter { snackType = SnackType.Chicken, dogName = "GameEngineProgramming" });
    //    dogCharacters.Add(new DogCharacter { snackType = SnackType.Meat, dogName = "Iwanttosleep" });
    //    dogCharacters.Add(new DogCharacter { snackType = SnackType.Toy, dogName = "CocaCola" });
    //}

    //public string GetDogName(SnackType type)
    //{
    //    var dog = dogCharacters.Find(d => d.snackType == type);
    //    return dog != null ? dog.dogName : "???";
    //}

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        totalCoins = Mathf.Max(0, totalCoins);  // 0 미만 방지
        OnCoinChanged?.Invoke(totalCoins);
    }

    //public void AddSnack(SnackType snack, int amount = 1)
    //{
    //    if (snackInventory.ContainsKey(snack))
    //    {
    //        snackInventory[snack] += amount;
    //    }
    //    else
    //    {
    //        snackInventory[snack] = amount;
    //    }
    //}
    public bool SpendCoin(int amount)
    {
        if(totalCoins>=amount)
        {
            totalCoins -= amount;
            OnCoinChanged?.Invoke(totalCoins);
            return true;

        }
        return false;

    }
    public int GetCoins()
    {
        return totalCoins;
    }
    public void AddOxygen(int amount)
    {
        oxygenPercent += amount;
        oxygenPercent = Mathf.Clamp(oxygenPercent + amount, 0, 100);
    }

    public void SetCurrentLoot(ResourceType type, int amount = 0)
    {
        currentResourceType = type;
        currentSnack = null;

        if (type == ResourceType.Coin)
        {
            coinAmount = amount;
        }
        else if (type == ResourceType.Oxygen)
        {
            oxygenPercent = amount;
        }
    }

    public void SetCurrentLoot(ResourceType type, SnackType snack)
    {
        currentResourceType = type;
        currentSnack = snack;
    }

    public void OpenBox(int currentDay)
    {
        int rand = Random.Range(0, 100);

        // 강아지 이름 짓기용 간식
        //if (rand < 3)
        //{
        //    AddSnack(SnackType.Bone);
        //    SetCurrentLoot(ResourceType.Snack, SnackType.Bone);
        //}
        //else if (rand < 6)
        //{
        //    AddSnack(SnackType.Chicken);
        //    SetCurrentLoot(ResourceType.Snack, SnackType.Chicken);
        //}
        //else if (rand < 9)
        //{
        //    AddSnack(SnackType.Meat);
        //    SetCurrentLoot(ResourceType.Snack, SnackType.Meat);
        //}
        //else if (rand < 12)
        //{
        //    AddSnack(SnackType.Toy);
        //    SetCurrentLoot(ResourceType.Snack, SnackType.Toy);
        //}
        
        if (rand < 20)
        {
            AddOxygen(10);
            SetCurrentLoot(ResourceType.Oxygen, 10);
        }
        else
        {
            int coins = Random.Range(10 + currentDay * 2, 15 + currentDay * 3);
            AddCoin(coins);
            SetCurrentLoot(ResourceType.Coin, coins);
        }
    }
}
