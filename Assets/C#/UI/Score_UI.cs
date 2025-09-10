using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    public int CurrentScore { get; private set; } = 0;

    [SerializeField] private Text ScoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int Amount)
    {
        CurrentScore += Amount;

        if (ScoreText != null)
            ScoreText.text = "Score : " + CurrentScore.ToString();
    }

    public int GetScore()
    {
        return CurrentScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        if (ScoreText != null)
            ScoreText.text = "Score : 0";
    }
}
