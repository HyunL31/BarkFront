using UnityEngine;
using System.Collections.Generic;

public class TypingChallengeManager : MonoBehaviour
{
    public static TypingChallengeManager Instance;

    [System.Serializable]
    public class DogChallenge
    {
        public int dogIndex;
        public List<string> challengeSentences;
    }

    public List<DogChallenge> dogChallenges;

    public bool lastBuffResult = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetBuffResult(bool success)
    {
        lastBuffResult = success;
    }

    public string GetRandomChallenge(int dogIndex)
    {
        foreach (var dog in dogChallenges)
        {
            if (dog.dogIndex == dogIndex)
            {
                if (dog.challengeSentences.Count == 0)
                    return "문장이 없습니다";
                return dog.challengeSentences[Random.Range(0, dog.challengeSentences.Count)];
            }
        }
        return "강아지 인덱스 오류";
    }
}
