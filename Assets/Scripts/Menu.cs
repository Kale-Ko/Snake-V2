using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Start()
    {
        Input.backButtonLeavesApp = false;
        Input.simulateMouseWithTouches = true;
    }

    public static void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public static void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().Pause();
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}