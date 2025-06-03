using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DogSpawner : MonoBehaviour
{
    public GameObject[] dogPrefabs;             // 강아지 프리팹 4개
    public Toggle[] toggles;                    // 토글 버튼 4개
    public TMP_Text challengeText;              // 챌린지 문장 표시용 텍스트
    public TMP_Text timerText;                  // 타이머 표시용 텍스트
    public TMP_InputField challengeInput;       // 문장 입력용 InputField
    public Tilemap tilemap;                     // 타일맵
    public ToggleGroup toggleGroup;             // ToggleGroup을 에디터에서 할당

    private int selectedIndex = -1;             // 선택된 강아지 인덱스
    private Dictionary<Vector2Int, GameObject> spawnedGroups = new Dictionary<Vector2Int, GameObject>();

    private float challengeTime = 5f;           // 제한 시간 5초
    private float timer = 0f;
    private bool isChallengeActive = false;     // 챌린지 진행 중 여부
    private bool isSpawnReady = false;          // 스폰 가능 상태 여부
    private string currentChallenge;            // 현재 챌린지 문장

void Start()
{
    for (int i = 0; i < toggles.Length; i++)
    {
        int idx = i;
        toggles[i].group = toggleGroup; // ToggleGroup에 할당
        toggles[i].onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                // 이미 선택된 토글이면(즉, 자동 해제 후 다시 켜지는 경우) 아무것도 하지 않음
                if (selectedIndex == idx)
                    return;

                selectedIndex = idx;
                currentChallenge = TypingChallengeManager.Instance.GetRandomChallenge(idx);
                challengeText.text = currentChallenge;
                timerText.text = "";
                challengeInput.text = "";
                challengeInput.gameObject.SetActive(true);
                challengeInput.Select();
                StartChallenge();
            }
            else if (selectedIndex == idx)
            {
                selectedIndex = -1;
                challengeText.text = "";
                timerText.text = "";
                challengeInput.gameObject.SetActive(false);
                isChallengeActive = false;
                isSpawnReady = false;
            }
        });
    }
    challengeInput.onEndEdit.AddListener(OnChallengeEndEdit);
    challengeInput.gameObject.SetActive(false);
}

    void StartChallenge()
    {
        timer = challengeTime;
        isChallengeActive = true;
        isSpawnReady = false;
    }

    void OnChallengeEndEdit(string input)
    {
        if (!isChallengeActive || selectedIndex == -1)
            return;

        if (input == currentChallenge)
        {
            // 타이핑 성공!
            isChallengeActive = false;
            isSpawnReady = true;
            challengeText.text = "";
            timerText.text = "";
            challengeInput.gameObject.SetActive(false);
        }
        else
        {
            // 타이핑 실패
            challengeInput.text = "";
        }
    }

    void Update()
    {
        // 타이머 처리
        if (isChallengeActive)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1") + "sec";
            if (timer <= 0f)
            {
                isChallengeActive = false;
                isSpawnReady = false;
                timerText.text = "Time Out!";
                challengeText.text = "";
                challengeInput.gameObject.SetActive(false);
            }
        }

        // 타이핑 챌린지 성공 후, 마우스 클릭 시 강아지 스폰
        if (Input.GetMouseButtonDown(0) && selectedIndex != -1 && isSpawnReady)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            SpawnDogIfChallengeSucceeded();
        }
    }

        void SpawnDogIfChallengeSucceeded()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - tilemap.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);

        if (!tilemap.cellBounds.Contains(tilePos))
            return;

        // 2x2 구역의 "중심" 타일 좌표 계산
        int groupSize = 2;
        int groupX = Mathf.FloorToInt((float)tilePos.x / groupSize);
        int groupY = Mathf.FloorToInt((float)tilePos.y / groupSize);
        Vector2Int groupKey = new Vector2Int(groupX, groupY);

        // 2x2 구역의 중심 타일 좌표
        int centerTileX = groupX * groupSize + groupSize / 2;
        int centerTileY = groupY * groupSize + groupSize / 2;
        Vector3Int centerTilePos = new Vector3Int(centerTileX, centerTileY, tilePos.z);

        if (!tilemap.cellBounds.Contains(centerTilePos))
            return;

        if (!spawnedGroups.ContainsKey(groupKey))
        {
            Vector3 spawnPos = tilemap.GetCellCenterWorld(centerTilePos);
            GameObject newDog = Instantiate(dogPrefabs[selectedIndex], spawnPos, Quaternion.identity);
            spawnedGroups.Add(groupKey, newDog);

            // 스폰 후 초기 상태로 복귀
            isSpawnReady = false;
            challengeText.text = "";
            timerText.text = "";
            challengeInput.gameObject.SetActive(false);

            foreach (var toggle in toggles)
            {
                toggle.isOn = false;
            }
            selectedIndex = -1;
        }
    }
}
