using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DogSpawner : MonoBehaviour
{
    public GameObject[] dogPrefabs;
    public Toggle[] toggles;
    public TMP_Text challengeText;
    public TMP_Text timerText;
    public TMP_InputField challengeInput;
    public Tilemap tilemap;
    public ToggleGroup toggleGroup;

    private int selectedIndex = -1;
    private Dictionary<Vector2Int, GameObject> spawnedGroups = new Dictionary<Vector2Int, GameObject>();

    private float challengeTime = 5f;
    private float timer = 0f;
    private bool isChallengeActive = false;
    private bool isSpawnReady = false;
    private string currentChallenge;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int idx = i;
            toggles[i].group = toggleGroup;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    if (selectedIndex == idx) return;

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
        if (!isChallengeActive || selectedIndex == -1) return;

        if (input == currentChallenge)
        {
            isChallengeActive = false;
            isSpawnReady = true;
            challengeText.text = "";
            timerText.text = "";
            challengeInput.gameObject.SetActive(false);
        }
        else
        {
            challengeInput.text = "";
        }
    }

    void Update()
    {
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

        if (Input.GetMouseButtonDown(0) && selectedIndex != -1 && isSpawnReady)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            SpawnDogWithImprovedLogic();
        }
    }

    void SpawnDogWithImprovedLogic()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - tilemap.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // 클릭 위치 보정 → 가장 가까운 타일 중심 기준으로 고정
        Vector3Int baseTilePos = tilemap.WorldToCell(worldPos);
        Vector3 baseCenter = tilemap.GetCellCenterWorld(baseTilePos);
        baseCenter.z = 0;

        // 오른쪽 타일도 계산
        Vector3Int rightTilePos = new Vector3Int(baseTilePos.x + 1, baseTilePos.y, baseTilePos.z);

        // 키 계산: x를 2칸 단위로 묶음
        Vector2Int groupKey = new Vector2Int(baseTilePos.x / 2, baseTilePos.y);
        if (spawnedGroups.ContainsKey(groupKey))
            return;

        Vector3 spawnPos;

        if (tilemap.HasTile(baseTilePos) && tilemap.HasTile(rightTilePos))
        {
            // 양쪽 다 존재하면 중앙에 소환
            Vector3 rightCenter = tilemap.GetCellCenterWorld(rightTilePos);
            spawnPos = (baseCenter + rightCenter) / 2f;
        }
        else if (tilemap.HasTile(baseTilePos))
        {
            // 왼쪽 하나만 있을 때 그 자리에 소환
            spawnPos = baseCenter;
        }
        else
        {
            // 아예 타일 없음
            return;
        }

        spawnPos.z = 0;
        GameObject dog = Instantiate(dogPrefabs[selectedIndex], spawnPos, Quaternion.identity);
        spawnedGroups.Add(groupKey, dog);

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

