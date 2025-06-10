using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvoidCCTV : MonoBehaviour
{
    [Header("감시 오브젝트")]
    public GameObject targetCirclePrefab;
    public int numberOfTargets = 3;
    public float moveSpeed = 5f;
    public float detectionRadius = 1.5f;

    [Header("UI 및 플레이어")]
    public Transform playerTransform;
    public Button evadeButton;
    public GameObject snapshotPanel;
    public Image snapshotImage;

    [Header("게임 설정")]
    public float gameDuration = 10f;
    public AudioSource alert;

    [Header("결과 패널")]
    public GameObject successPanel;
    public GameObject failPanel;

    private List<Transform> targetObjects = new List<Transform>();
    private List<Vector3> targetDestinations = new List<Vector3>();

    private bool gameEnded = false;

    public static AvoidCCTV Instance;

    public bool IsPlayingCCTVGame { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        snapshotPanel.SetActive(false);
        evadeButton.gameObject.SetActive(true);
        gameEnded = false;

        // 기존 CCTV 제거
        foreach (var obj in targetObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        targetObjects.Clear();
        targetDestinations.Clear();

        // CCTV 생성
        for (int i = 0; i < numberOfTargets; i++)
        {
            var obj = Instantiate(targetCirclePrefab);
            targetObjects.Add(obj.transform);
            targetDestinations.Add(GetRandomWorldPosition());
        }

        evadeButton.onClick.RemoveAllListeners();
        evadeButton.onClick.AddListener(() => TryEvade());

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        IsPlayingCCTVGame = true;

        alert.Play();

        float timer = 0f;

        while (timer < gameDuration)
        {
            for (int i = 0; i < targetObjects.Count; i++)
            {
                MoveTarget(i);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 타이머 초과 실패
        if (!gameEnded)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
            StartCoroutine(CaptureAndShowSnapshot(screenPos));
            StartCoroutine(HandleFailure(screenPos));
            EndGame();
        }
    }

    void MoveTarget(int index)
    {
        Transform target = targetObjects[index];
        Vector3 destination = targetDestinations[index];

        target.position = Vector3.MoveTowards(target.position, destination, moveSpeed * Time.deltaTime);

        // 목적지 도달 시 새로운 랜덤 위치 지정
        if (Vector3.Distance(target.position, destination) < 0.1f)
        {
            targetDestinations[index] = GetRandomWorldPosition();
        }
    }

    Vector3 GetRandomWorldPosition()
    {
        Vector2 randomScreenPos = new Vector2(
            Random.Range(50f, Screen.width - 50f),
            Random.Range(50f, Screen.height - 50f)
        );

        return Camera.main.ScreenToWorldPoint(new Vector3(randomScreenPos.x, randomScreenPos.y, 10f));
    }
    void TryEvade()
    {
        if (gameEnded) return;

        Vector3 playerPos = playerTransform.position;

        // 실패 판정
        foreach (Transform target in targetObjects)
        {
            if (Vector3.Distance(playerPos, target.position) < detectionRadius)
            {
                ResourceManager.Instance.AddCoin(-10);

                Vector2 screenPos = Camera.main.WorldToScreenPoint(playerPos);
                StartCoroutine(HandleFailure(screenPos));
                return;
            }
        }

        // 성공 판정
        StartCoroutine(HandleSuccess());
    }

    void EndGame()
    {
        IsPlayingCCTVGame = false;

        gameEnded = true;
        evadeButton.gameObject.SetActive(false);

        foreach (var obj in targetObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        targetObjects.Clear();
        alert.Stop();
    }

    // 실패 시 플레이어 캡처
    IEnumerator CaptureAndShowSnapshot(Vector2 screenPos)
    {
        yield return new WaitForEndOfFrame();

        Texture2D screen = ScreenCapture.CaptureScreenshotAsTexture();

        int size = 200;
        int x = Mathf.Clamp((int)screenPos.x - size / 2, 0, screen.width - size);
        int y = Mathf.Clamp((int)screenPos.y - size / 2, 0, screen.height - size);

        Texture2D cropped = new Texture2D(size, size);
        cropped.SetPixels(screen.GetPixels(x, y, size, size));
        cropped.Apply();

        Destroy(screen);

        snapshotImage.sprite = Sprite.Create(cropped, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        snapshotPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        snapshotPanel.SetActive(false);
    }

    IEnumerator HandleFailure(Vector2 screenPos)
    {
        yield return StartCoroutine(CaptureAndShowSnapshot(screenPos));

        if (failPanel != null)
        {
            failPanel.SetActive(true);
        }

        EndGame();

        yield return new WaitForSeconds(2f);

        if (failPanel != null)
        {
            failPanel.SetActive(false);
        }
    }

    IEnumerator HandleSuccess()
    {
        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }

        EndGame();

        yield return new WaitForSeconds(2f);

        if (successPanel != null)
        {
            successPanel.SetActive(false);
        }
    }
}