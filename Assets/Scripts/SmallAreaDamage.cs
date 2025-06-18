using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAreaDamage : MonoBehaviour
{
    public int damageAmount = 15;
    public float damageInterval = 0.4f;
    public string targetTag = "robot";

    public GameObject effectObject; // 파티클 이펙트 오브젝트

    private HashSet<RobotController> robotsInRange = new HashSet<RobotController>();
    private Coroutine damageRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            RobotController robot = other.GetComponent<RobotController>();
            if (robot != null)
            {
                robotsInRange.Add(robot);

                if (damageRoutine == null)
                {
                    damageRoutine = StartCoroutine(DamageLoop());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            RobotController robot = other.GetComponent<RobotController>();
            if (robot != null)
            {
                robotsInRange.Remove(robot);

                if (robotsInRange.Count == 0 && damageRoutine != null)
                {
                    StopCoroutine(damageRoutine);
                    damageRoutine = null;

                    if (effectObject != null)
                        effectObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator DamageLoop()
    {
        while (robotsInRange.Count > 0)
        {
            // 이펙트 켜기
            if (effectObject != null)
                effectObject.SetActive(true);

            // 데미지 주기
            foreach (var robot in robotsInRange)
            {
                if (robot != null)
                    robot.TakeDamage(damageAmount);
            }

            yield return new WaitForSeconds(0.2f);

            // 이펙트 끄기
            if (effectObject != null)
                effectObject.SetActive(false);

            yield return new WaitForSeconds(0.2f); // 나머지 0.2초
        }
    }
}
