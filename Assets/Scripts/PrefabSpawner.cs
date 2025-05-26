using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float accuracyThreshold = 0.5f;

    void Update()
    {
        WhisperRunner wr = FindAnyObjectByType<WhisperRunner>();

        if (wr == null || !wr.isReady)
            return; // 아직 준비 안 됨

        if (Input.GetMouseButtonDown(0))
        {
            if (wr.accuracy >= accuracyThreshold)
            {
                Vector3 spawnPos = GetClickWorldPosition2D();
                Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                Debug.Log("✅ 프리팹 소환됨");
            }
            else
            {
                Debug.Log("❌ 정확도 부족으로 소환되지 않음");
            }
        }
    }


    Vector3 GetClickWorldPosition2D()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Z축 제거
        return mousePos;
    }
}
