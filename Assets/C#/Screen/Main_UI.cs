using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Screen : MonoBehaviour
{
    public GameObject HelpPanel;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Game_Start()
    {
        SceneManager.LoadScene("Game"); 
    }

    public void Help()
    {
        HelpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        HelpPanel.SetActive(false);
    }

    public void Done()
    {
        Application.Quit();
    }
}
