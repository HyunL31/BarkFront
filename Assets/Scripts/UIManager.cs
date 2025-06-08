using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ResourceUI resourceUI;
    public GameObject resourceUIPanel;

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

        StartCoroutine(HideResourceUI());
    }

    IEnumerator HideResourceUI()
    {
        yield return new WaitForSeconds(3f);

        resourceUIPanel.SetActive(false);
    }
}