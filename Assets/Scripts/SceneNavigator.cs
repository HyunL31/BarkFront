using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Opening");
    }

    public void Skip()
    {
        SceneManager.LoadScene("Day");
    }
}
