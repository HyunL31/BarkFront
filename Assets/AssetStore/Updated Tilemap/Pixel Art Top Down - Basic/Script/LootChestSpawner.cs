using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LootChestSpawner:
///  • 씬(Scene) 시작 시, inspector에 등록된 위치들(Transform 목록)에 한 번만
///    LootChest(prefab)를 Instantiate 합니다.
///  • 각 체스트 인스턴스는 스스로 “열리고 닫히고”를 반복하므로, 스포너는 최초 생성 역할만 담당합니다.
/// </summary>
public class LootChestSpawner : MonoBehaviour
{
    [Header("1) LootChest Prefab")]
    [Tooltip("LootChest.prefab (LootChest.cs, LootResult.cs가 붙어 있는 프리팹)")]
    public GameObject lootChestPrefab;

    [Space]
    [Header("2) 체스트를 생성할 위치 목록")]
    [Tooltip("씬에서 미리 만들어 둔 빈 GameObject(예: SpawnPoint1, SpawnPoint2…)의 Transform 목록")]
    public List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        // 씬이 시작되면 spawnPoints 리스트에 들어 있는 모든 위치에 한 번만 루트 체스트 생성
        foreach (Transform t in spawnPoints)
        {
            SpawnChestAt(t.position);
        }
    }

    /// <summary>
    /// 특정 위치(position)에 LootChest 하나를 즉시 생성합니다.
    /// </summary>
    private void SpawnChestAt(Vector3 position)
    {
        if (lootChestPrefab == null)
        {
            Debug.LogWarning("[LootChestSpawner] lootChestPrefab이 연결되지 않았습니다!");
            return;
        }

        // 1) 체스트 프리팹을 복제
        GameObject go = Instantiate(lootChestPrefab, position, Quaternion.identity);

        // 2) 생성된 오브젝트에 LootChest 컴포넌트가 붙어 있는지 확인
        LootChest lc = go.GetComponent<LootChest>();
        if (lc == null)
        {
            Debug.LogWarning($"[LootChestSpawner] 생성된 오브젝트에 LootChest 컴포넌트가 없습니다! ({go.name})");
            return;
        }

        // (선택) 필요하다면 spawnTime 범위를 개별 체스트마다 다르게 설정할 수 있습니다:
        // 예를 들어, SpawnPoint마다 서로 다른 min/max 값을 주고 싶다면 여기서 lc.minRespawnTime = …; 등을 세팅하세요.
    }
}
