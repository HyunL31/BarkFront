using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void StartButton(GameObject startPanel)
    {
        startPanel.SetActive(false);
    }

    public void OpeningStart(GameObject cutscene)
    {
        cutscene.SetActive(true);
    }
}
