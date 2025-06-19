using System.Collections;
using UnityEngine;

/// <summary>
/// 루트 박스 스크립트
/// </summary>

public class LootChest : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite openedChestSprite;
    public Sprite closedChestSprite;

    [Header("Respawn Timing")]
    public float minRespawnTime = 100f;
    public float maxRespawnTime = 200f;

    private bool isOpened = false;
    public bool IsOpened => isOpened;

    private SpriteRenderer spriteRenderer;
    private Collider2D chestCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chestCollider = GetComponent<Collider2D>();

        chestCollider.isTrigger = true;

        if (closedChestSprite != null)
        {
            spriteRenderer.sprite = closedChestSprite;
        }
    }

    public void OpenChest()
    {
        if (isOpened)
        {
            return;
        }

        isOpened = true;

        if (openedChestSprite != null)
        {
            spriteRenderer.sprite = openedChestSprite;
        }

        LootResult loot = GetComponent<LootResult>();

        if (loot != null)
        {
            loot.GiveLoot();
        }

        chestCollider.enabled = false;
        float delay = Random.Range(minRespawnTime, maxRespawnTime);
        StartCoroutine(CloseAfterDelay(delay));
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (closedChestSprite != null)
        {
            spriteRenderer.sprite = closedChestSprite;
        }

        isOpened = false;
        chestCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player pm = other.GetComponent<Player>();

            if (pm != null)
            {
                pm.SetTargetChest(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player pm = other.GetComponent<Player>();

            if (pm != null)
            {
                pm.ClearTargetChest();
            }
        }
    }
}
