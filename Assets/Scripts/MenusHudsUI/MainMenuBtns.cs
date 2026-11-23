using UnityEngine;

public class MainMenuBtns : MonoBehaviour
{
    public void StartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel_1");
    }

    public void ExitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}