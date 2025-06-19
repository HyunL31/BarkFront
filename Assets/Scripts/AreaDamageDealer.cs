using System.Collections;
using UnityEngine;

public class AreaDamageDealer : MonoBehaviour
{
    public float interval = 5f;
    public int damage = 100;

    public GameObject damageAllEffect; // "DamageAll" 파티클 오브젝트
    public float effectDuration = 1.5f;

    private void Start()
    {
        StartCoroutine(DamageLoop());
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // 파티클 이펙트 켜기
            if (damageAllEffect != null)
            {
                damageAllEffect.SetActive(true);
                StartCoroutine(DisableEffectAfterDelay(effectDuration));
            }

            // 데미지 적용
            ApplyDamageToAllRobots();
        }
    }

    void ApplyDamageToAllRobots()
    {
        // 태그가 "Robot"이거나 레이어가 "Robot"인 모든 오브젝트 찾아서 데미지
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if ( obj.layer == LayerMask.NameToLayer("Robot")&& obj.activeInHierarchy)
            {
                RobotController robot = obj.GetComponent<RobotController>();
                if (robot != null )
                {
                    robot.TakeDamage(damage);
                }
            }
        }
    }

    IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (damageAllEffect != null)
            damageAllEffect.SetActive(false);
    }
}
