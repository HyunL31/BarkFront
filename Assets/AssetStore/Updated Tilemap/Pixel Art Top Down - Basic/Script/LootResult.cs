using UnityEngine;

public class LootResult : MonoBehaviour
{
    public enum LootType { Robot, Coin, DogItemA, DogItemB, DogItemC, DogItemD }

    public int currentDay = 1;
    public void GiveLoot()
    {
        LootType result = GetRandomLoot();

        switch (result)
        {
            case LootType.Robot:
                Debug.Log("미니게임으로 이동!");
                // ��: FindObjectOfType<GameManager>().TriggerRobotMiniGame();
                break;

            case LootType.Coin:
                int coins = GetCoinAmount();
                Debug.Log($"{coins}의 코인 획득!");
                break;

            case LootType.DogItemA:
            case LootType.DogItemB:
            case LootType.DogItemC:
            case LootType.DogItemD:
                Debug.Log($"{result} 아이템 회득!");
                // 아이템을 획득 개수 / 전체 개수도 띄울지는 고민
                break;
        }
    }

    private LootType GetRandomLoot()
    {
        int rand = Random.Range(0, 100);

        if (rand < 3) return LootType.DogItemA;         // 3%
        else if (rand < 6) return LootType.DogItemB;    // 3%
        else if (rand < 9) return LootType.DogItemC;   // 3%
        else if (rand < 12) return LootType.DogItemD;   // 3%
        else if (rand < 42) return LootType.Robot;      // 30%
        else return LootType.Coin;                      // 58%
    }
    private int GetCoinAmount()
    {
        // 날이 지날수록 얻는 코인이 많아지도록 조정
        return Random.Range(10 + currentDay * 2, 15 + currentDay * 3);
    }

    //private int GetRobotMiniGameIndex()
    //{
    //    //미니 게임의 개수 일단은 한 개 
    //    return Random.Range(minInclusive: 0, 3);
    //}
    public void NextDay()
    { // 나중에 따로 구현!!
        currentDay++;
    }
}

