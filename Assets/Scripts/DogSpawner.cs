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

    public AudioSource spawnSound;

    private int selectedIndex = -1;
    private Dictionary<Vector2Int, GameObject> spawnedGroups = new Dictionary<Vector2Int, GameObject>();

    private float challengeTime = 5f;
    private float timer = 0f;
    private bool isChallengeActive = false;
    private bool isSpawnReady = false;
    private string currentChallenge;

    public AudioSource successClip;
    public AudioSource failClip;

    //private GameObject buffEffectObject;

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

        challengeInput.onValueChanged.AddListener(UpdateTypingEffect); // 실시간 입력 반영
        challengeInput.onEndEdit.AddListener(OnChallengeEndEdit);
        challengeInput.gameObject.SetActive(false);
    }

    void StartChallenge()
    {
        timer = challengeTime;
        isChallengeActive = true;
        isSpawnReady = false;
        UpdateTypingEffect(""); // 초기화
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
            challengeText.text = $"<color=#ff0000>{typed}</color>"; // 틀릴 경우 붉게
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
            successClip.Play();
            challengeInput.gameObject.SetActive(false);
        }
        else
        {
            challengeInput.text = ""; // 다시 시도
            failClip.Play();
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
        if (!ResourceManager.Instance.SpendCoin(dogCosts[selectedIndex]))
        {
            Debug.Log("코인이 부족해서 강아지를 소환할 수 없습니다.");
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
        if (spawnedGroups.ContainsKey(groupKey))
            return;

        if (!(tilemap.HasTile(leftTilePos) && tilemap.HasTile(rightTilePos)))
            return;

        Vector3 leftCenter = tilemap.GetCellCenterWorld(leftTilePos);
        Vector3 rightCenter = tilemap.GetCellCenterWorld(rightTilePos);
        Vector3 spawnPos = (leftCenter + rightCenter) / 2f;
        spawnPos.z = 0;

        GameObject dog = Instantiate(dogPrefabs[selectedIndex], spawnPos, Quaternion.identity);
        spawnSound.Play();
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
}
