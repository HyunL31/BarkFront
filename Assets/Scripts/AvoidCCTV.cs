using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvoidCCTV : MonoBehaviour
{
    [Header("감시 오브젝트")]
    public GameObject targetCirclePrefab;             // CCTV 프리팹
    public int numberOfTargets = 3;                   // CCTV 수
    public float moveSpeed = 5f;                      // 이동 속도
    public float detectionRadius = 1.5f;              // 감지 범위

    [Header("UI 및 플레이어")]
    public Transform playerTransform;
    public Button evadeButton;
    public GameObject snapshotPanel;
    public Image snapshotImage;

    [Header("게임 설정")]
    public float gameDuration = 10f;
    public AudioSource alert;

    private List<Transform> targetObjects = new List<Transform>();
    private List<Vector3> targetDestinations = new List<Vector3>();

    private bool gameEnded = false;

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

        if (!gameEnded)
        {
            Debug.Log("타이머 초과 실패");
            Vector2 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
            StartCoroutine(CaptureAndShowSnapshot(screenPos));
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
        if (gameEnded)
        {
            return;
        }

        Vector3 playerPos = playerTransform.position;

        // CCTV 중 하나라도 범위 안이면 실패
        foreach (Transform target in targetObjects)
        {
            if (Vector3.Distance(playerPos, target.position) < detectionRadius)
            {
                Debug.Log("실패");
                Vector2 screenPos = Camera.main.WorldToScreenPoint(playerPos);
                StartCoroutine(CaptureAndShowSnapshot(screenPos));
                EndGame();
                return;
            }
        }

        Debug.Log("성공");
        EndGame();
    }

    void EndGame()
    {
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
}