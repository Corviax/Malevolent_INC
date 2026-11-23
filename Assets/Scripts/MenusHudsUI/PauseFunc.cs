using UnityEngine;

public class PauseFunc : MonoBehaviour
{
    public GameObject container;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
