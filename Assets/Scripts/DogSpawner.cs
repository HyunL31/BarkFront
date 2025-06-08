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

    private GameObject buffEffectObject;
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

        bool isCorrect = input == currentChallenge;

        isChallengeActive = false;
        isSpawnReady = true;

        // ✅ 성공 여부 기록
        TypingChallengeManager.Instance.SetBuffResult(isCorrect);

        // UI 초기화
        challengeText.text = "";
        timerText.text = "";
        challengeInput.gameObject.SetActive(false);
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
                isSpawnReady = true; // ✅ 시간 초과도 실패로 간주하고 소환 가능하게
                TypingChallengeManager.Instance.SetBuffResult(false);

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

        Vector3Int baseTilePos = tilemap.WorldToCell(worldPos);
        int groupX = baseTilePos.x % 2 == 0 ? baseTilePos.x : baseTilePos.x - 1;
        Vector3Int leftTilePos = new Vector3Int(groupX, baseTilePos.y, baseTilePos.z);
        Vector3Int rightTilePos = new Vector3Int(groupX + 1, baseTilePos.y, baseTilePos.z);

        Vector2Int groupKey = new Vector2Int(groupX / 2, baseTilePos.y);
        if (spawnedGroups.ContainsKey(groupKey)) return;
        if (!(tilemap.HasTile(leftTilePos) && tilemap.HasTile(rightTilePos))) return;

        Vector3 leftCenter = tilemap.GetCellCenterWorld(leftTilePos);
        Vector3 rightCenter = tilemap.GetCellCenterWorld(rightTilePos);
        Vector3 spawnPos = (leftCenter + rightCenter) / 2f;
        spawnPos.z = 0;

        GameObject dog = Instantiate(dogPrefabs[selectedIndex], spawnPos, Quaternion.identity);
        dog.tag = "dog";
        spawnedGroups.Add(groupKey, dog);

        // ✅ 강화 여부 적용
        DogController controller = dog.GetComponent<DogController>();
        if (controller != null)
        {
            controller.isBuffed = TypingChallengeManager.Instance.lastBuffResult;
        }

        if (controller.isBuffed)
    {
        Transform buffEffect = dog.transform.Find("Buff");
        if (buffEffect != null)
        {
            buffEffect.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Buff 파티클이 Dog 프리팹에 없습니다.");
        }
    }

        // UI 및 상태 초기화
        isSpawnReady = false;
        challengeText.text = "";
        timerText.text = "";
        challengeInput.gameObject.SetActive(false);
        foreach (var toggle in toggles) toggle.isOn = false;
        selectedIndex = -1;
    }

    private void OnDrawGizmos()
    {
        if (tilemap == null) return;
        BoundsInt bounds = tilemap.cellBounds;
        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x <= bounds.xMax; x += 2)
            {
                Vector3Int leftTile = new Vector3Int(x, y, 0);
                Vector3Int rightTile = new Vector3Int(x + 1, y, 0);
                if (tilemap.HasTile(leftTile) && tilemap.HasTile(rightTile))
                {
                    Vector3 leftCenter = tilemap.GetCellCenterWorld(leftTile);
                    Vector3 rightCenter = tilemap.GetCellCenterWorld(rightTile);
                    Vector3 center = (leftCenter + rightCenter) / 2f;
                    center.z = 0;

                    Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.3f);
                    Gizmos.DrawCube(center, new Vector3(1f, 1f, 0.1f));

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireCube(center, new Vector3(1f, 1f, 0.1f));
                }
            }
        }
    }
}
