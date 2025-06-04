using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvoidCCTV : MonoBehaviour
{
    [Header("감시 오브젝트")]
    public GameObject targetCirclePrefab;
    public int numberOfTargets = 3;
    public float moveSpeed = 3f;
    public float detectionRadius = 1.5f;

    [Header("UI 및 플레이어")]
    public Transform playerTransform;
    public Button evadeButton;
    public GameObject snapshotPanel;
    public Image snapshotImage;

    [Header("게임 설정")]
    public float gameDuration = 15f;

    private List<Transform> targetObjects = new List<Transform>();
    private List<Vector3> targetDestinations = new List<Vector3>();

    private bool gameEnded = false;

    void OnEnable()
    {
        snapshotPanel.SetActive(false);
        evadeButton.gameObject.SetActive(true);
        gameEnded = false;

        // 기존 CCTV 오브젝트 제거
        foreach (var obj in targetObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        targetObjects.Clear();
        targetDestinations.Clear();

        // CCTV 오브젝트 생성 및 목적지 설정
        for (int i = 0; i < numberOfTargets; i++)
        {
            var obj = Instantiate(targetCirclePrefab);
            targetObjects.Add(obj.transform);
            targetDestinations.Add(GetRandomWorldPosition());
        }

        // 버튼 클릭 리스너 초기화 및 등록
        evadeButton.onClick.RemoveAllListeners();
        evadeButton.onClick.AddListener(() => TryEvade());

        // 게임 시작 코루틴 실행
        StartCoroutine(StartGame());
    }

    // 게임 타이머 및 CCTV 이동을 관리하는 코루틴
    IEnumerator StartGame()
    {
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

        // 시간 초과 시 실패
        if (!gameEnded)
        {
            Debug.Log("타이머 초과 실패");
            Vector2 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
            StartCoroutine(CaptureAndShowSnapshot(screenPos));
            EndGame();
        }
    }

    // 특정 CCTV를 목적지 방향으로 이동시키는 함수
    void MoveTarget(int index)
    {
        Transform target = targetObjects[index];
        Vector3 destination = targetDestinations[index];

        target.position = Vector3.MoveTowards(target.position, destination, moveSpeed * Time.deltaTime);

        // 목적지에 도달했다면 새 목적지를 지정
        if (Vector3.Distance(target.position, destination) < 0.1f)
        {
            targetDestinations[index] = GetRandomWorldPosition();
        }
    }

    // 화면 내 임의 위치를 월드 좌표로 변환하여 반환
    Vector3 GetRandomWorldPosition()
    {
        Vector2 randomScreenPos = new Vector2(
            Random.Range(50f, Screen.width - 50f),
            Random.Range(50f, Screen.height - 50f)
        );

        return Camera.main.ScreenToWorldPoint(new Vector3(randomScreenPos.x, randomScreenPos.y, 10f));
    }

    // 회피 버튼을 눌렀을 때 실행되는 함수
    void TryEvade()
    {
        if (gameEnded) return;

        Vector3 playerPos = playerTransform.position;

        // 감지 범위 내 CCTV가 있는지 확인
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

    // 게임 종료 
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

        // 게임 종료 후 부모 패널 끄기
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }

        // RobotEvent에 종료 알림
        FindAnyObjectByType<RobotEvent>()?.OnGameEnded?.Invoke();
    }

    // 실패 시 플레이어의 화면 일부를 캡처하여 보여주는 코루틴
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
    }
}
