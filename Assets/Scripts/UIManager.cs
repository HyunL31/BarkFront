using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ResourceUI resourceUI;
    public GameObject resourceUIPanel;

    public AudioSource achieveSound;

    [Header("Oxygen UI")]
    public Image oxygenFillImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        oxygenFillImage.color = Color.green;
    }

    void OnEnable()
    {
        OxygenSystem.OnOxygenChanged += UpdateOxygenUI;
    }

    void OnDisable()
    {
        OxygenSystem.OnOxygenChanged -= UpdateOxygenUI;
    }

    private void UpdateOxygenUI(float percent)
    {
        if (oxygenFillImage != null)
        {
            oxygenFillImage.fillAmount = percent;

            // 퍼센트에 따라 색상 설정
            if (percent > 0.6f)
            {
                oxygenFillImage.color = Color.green;
            }
            else if (percent > 0.3f)
            {
                oxygenFillImage.color = Color.yellow;
            }
            else
            {
                oxygenFillImage.color = Color.red;
            }
        }
    }

    public void ShowResourceUI()
    {
        if (resourceUI == null)
        {
            return;
        }

        if (resourceUIPanel == null)
        {
            return;
        }

        resourceUI.DisplayResource();
        resourceUIPanel.SetActive(true);
        achieveSound.Play();

        StartCoroutine(HideResourceUI());
    }

    IEnumerator HideResourceUI()
    {
        yield return new WaitForSeconds(3f);

        resourceUIPanel.SetActive(false);
    }
}