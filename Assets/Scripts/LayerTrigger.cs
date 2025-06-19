using UnityEngine;
using UnityEngine.Tilemaps;

namespace Cainos.PixelArtTopDown_Basic
{
    /// <summary>
    /// LayerTrigger:
    ///  • OnTriggerExit2D 시점에 “layer” / “sortingLayer” 필드로 지정된 레이어로
    ///    플레이어의 물리 Layer와 Sorting Layer를 바꿔 줍니다.
    ///  • 동시에 “wallEnable”에 연결된 타일맵 콜라이더만 켜고,
    ///    “wallDisable”에 연결된 타일맵 콜라이더는 끕니다.
    ///
    /// 사용법:
    ///  1) 씬에 계단 밑/계단 위에 둘 EdgeCollider2D(Is Trigger) 오브젝트를 만듭니다.
    ///  2) 각 오브젝트에 이 LayerTrigger 스크립트를 붙입니다.
    ///  3) Inspector에서 다음 필드를 채웁니다:
    ///     • layer          : 전환할 물리 Layer 이름 (예: "Layer 2")
    ///     • sortingLayer   : 전환할 Sprite Sorting Layer 이름 (예: "Layer 2")
    ///     • wallEnable     : 이 트리거를 통과하면 활성화할 “벽(Tilemap)” 오브젝트
    ///     • wallDisable    : 이 트리거를 통과하면 비활성화할 “벽(Tilemap)” 오브젝트
    ///
    ///  예를 들어,
    ///    - “계단 밑” 트리거에는 layer="Layer 2", sortingLayer="Layer 2",
    ///      wallEnable=“Layer 2 – Wall”, wallDisable=“Layer 1 – Wall” 로 설정하면,
    ///      플레이어가 계단 밑을 벗어나는 순간 1→2층으로 전환됩니다.
    ///
    ///    - “계단 위” 트리거에는 layer="Layer 1", sortingLayer="Layer 1",
    ///      wallEnable=“Layer 1 – Wall”, wallDisable=“Layer 2 – Wall” 로 설정하면,
    ///      플레이어가 계단 위를 벗어나는 순간 2→1층으로 복귀됩니다.
    /// </summary>
    public class LayerTrigger : MonoBehaviour
    {
        [Header("=== Player Layer & Sorting Layer 설정 ===")]
        [Tooltip("트리거를 통과할 때 플레이어가 전환될 물리 Layer 이름(예: \"Layer 1\", \"Layer 2\", \"Layer 3\")")]
        public string layer;

        [Tooltip("트리거를 통과할 때 플레이어가 전환될 Sorting Layer 이름(예: \"Layer 1\", \"Layer 2\", \"Layer 3\")")]
        public string sortingLayer;

        [Header("=== Wall Tilemap 오브젝트 (콜라이더 on/off) ===")]
        [Tooltip("트리거를 통과할 때 활성화할 벽(Tilemap) 오브젝트")]
        public GameObject wallEnable;

        [Tooltip("트리거를 통과할 때 비활성화할 벽(Tilemap) 오브젝트")]
        public GameObject wallDisable;

        private void OnTriggerExit2D(Collider2D other)
        {
            // 1) Player 태그가 아니라면 무시
            if (!other.CompareTag("Player"))
                return;

            // 2) 우선 물리 Layer 전환
            int newLayerIdx = LayerMask.NameToLayer(layer);
            if (newLayerIdx < 0)
            {
                Debug.LogWarning($"[LayerTrigger] 지정된 layer(\"{layer}\")가 없습니다.");
            }
            else
            {
                other.gameObject.layer = newLayerIdx;
            }

            // 3) SpriteRenderer의 Sorting Layer도 전환
            SpriteRenderer mainSr = other.gameObject.GetComponent<SpriteRenderer>();
            if (mainSr != null)
            {
                mainSr.sortingLayerName = sortingLayer;
            }
            SpriteRenderer[] childSrs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in childSrs)
            {
                sr.sortingLayerName = sortingLayer;
            }

            // 4) 벽 콜라이더 on/off 토글
            ToggleWallObject(wallEnable, true);
            ToggleWallObject(wallDisable, false);

            Debug.Log($"[LayerTrigger] 플레이어를 '{layer}'로 전환, '{wallEnable.name}' 활성화, '{wallDisable.name}' 비활성화");
        }

        /// <summary>
        /// 지정된 벽 오브젝트(GameObject)에 붙은 TilemapCollider2D와 CompositeCollider2D를
        /// enable=true/false에 따라 켜거나 끕니다.
        /// </summary>
        private void ToggleWallObject(GameObject wallObj, bool enable)
        {
            if (wallObj == null)
                return;

            var tilemapCol = wallObj.GetComponent<TilemapCollider2D>();
            var compositeCol = wallObj.GetComponent<CompositeCollider2D>();

            if (tilemapCol != null)
                tilemapCol.enabled = enable;

            if (compositeCol != null)
                compositeCol.enabled = enable;
        }
    }
}

