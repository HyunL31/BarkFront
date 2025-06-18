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
    public Image progressBar;
    public int[] dogCosts;

    public CoinUIManager coinManager; // 💰 코인 매니저 참조 (인스펙터에서 할당)

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

        challengeInput.onValueChanged.AddListener(UpdateTypingEffect);
        challengeInput.onEndEdit.AddListener(OnChallengeEndEdit);
        challengeInput.gameObject.SetActive(false);
    }

    void StartChallenge()
    {
        timer = challengeTime;
        isChallengeActive = true;
        isSpawnReady = false;
        UpdateTypingEffect("");
    }

    void UpdateTypingEffect(string typed)
    {
        if (!isChallengeActive || string.IsNullOrEmpty(currentChallenge)) return;

        if (currentChallenge.StartsWith(typed))
        {
            string rest = currentChallenge.Substring(typed.Length);
            challengeText.text = $"<b><color=#ffffff>{typed}</color></b><color=#888888>{rest}</color>";
            if (progressBar != null)
                progressBar.fillAmount = (float)typed.Length / currentChallenge.Length;
        }
        else
        {
            challengeText.text = $"<color=#ff0000>{typed}</color>";
            if (progressBar != null)
                progressBar.fillAmount = 0f;
        }
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
            timerText.text = timer.ToString("F1") + " sec";

            if (timer <= 0f)
            {
                isChallengeActive = false;
                isSpawnReady = true;
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
        // 💰 소환 비용 체크
        int cost = dogCosts[selectedIndex];
        if (coinManager.currentCoins < cost)
        {
            Debug.Log("코인이 부족합니다!");
            return;
        }

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

        // 💰 코인 차감
        coinManager.SpendCoins(cost);

        GameObject dog = Instantiate(dogPrefabs[selectedIndex], spawnPos, Quaternion.identity);
        dog.tag = "dog";
        spawnedGroups.Add(groupKey, dog);

        isSpawnReady = false;
        challengeText.text = "";
        timerText.text = "";
        challengeInput.gameObject.SetActive(false);
        progressBar.fillAmount = 0f;

        foreach (var toggle in toggles)
            toggle.isOn = false;

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


