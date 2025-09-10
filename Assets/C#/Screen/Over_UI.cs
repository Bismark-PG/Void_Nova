using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverPanel;
    public Text FinalScore;

    public void Game_Over()
    {
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (FinalScore != null)
            FinalScore.text = "Score : " + Score.Instance.GetScore().ToString();
    }

    public void Main()
    {
        Time.timeScale = 1f;
        Score.Instance.ResetScore();
        SceneManager.LoadScene("Title Scene");
    }

    public void RE()
    {
        Time.timeScale = 1f;
        Score.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Done()
    {
        Application.Quit();
    }
}
