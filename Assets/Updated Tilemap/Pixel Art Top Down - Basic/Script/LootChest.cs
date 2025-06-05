using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class LootChest : MonoBehaviour
{
    [Header("Sprites")]
    [Tooltip("체스트가 열렸을 때")]
    public Sprite openedChestSprite;

    [Tooltip("체스트가 닫혀 있을 때(기본 상태)")]
    public Sprite closedChestSprite;

    [Header("Respawn Timing")]
    [Tooltip("체스트를 연 뒤 최소 대기 시간")]
    public float minRespawnTime = 3f;

    [Tooltip("체스트를 연 뒤 최대 대기 시간")]
    public float maxRespawnTime = 10f;

    private bool isOpened = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D chestCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chestCollider = GetComponent<Collider2D>();

        chestCollider.isTrigger = true;

        if (closedChestSprite != null)
            spriteRenderer.sprite = closedChestSprite;
    }

    public void OpenChest()
    {
        if (isOpened)
            return;

        isOpened = true;

        if (openedChestSprite != null)
            spriteRenderer.sprite = openedChestSprite;

        LootResult loot = GetComponent<LootResult>();
        if (loot != null)
            loot.GiveLoot();

        // 체스트는 열린 상태 동안 다시 열리지 않도록 Collider 비활성화
        chestCollider.enabled = false;

        float delay = Random.Range(minRespawnTime, maxRespawnTime);
        StartCoroutine(CloseAfterDelay(delay));
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (closedChestSprite != null)
            spriteRenderer.sprite = closedChestSprite;

        isOpened = false;
        chestCollider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지됨");

            Cainos.PixelArtTopDown_Basic.Player pm = other.GetComponent<Cainos.PixelArtTopDown_Basic.Player>();
            if (pm != null) pm.SetTargetChest(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cainos.PixelArtTopDown_Basic.Player pm = other.GetComponent<Cainos.PixelArtTopDown_Basic.Player>();
            if (pm != null) pm.ClearTargetChest();
        }
    }
}
