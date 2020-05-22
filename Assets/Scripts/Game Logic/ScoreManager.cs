using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the current score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int currentLevel = 0;

    private Text scoreText;
    private Text levelText;

    public float fallTime = 0.8f;

    private AudioSource aSrc;
    public AudioClip clearLine, GameOver, levelUp;

    /// <summary>
    /// Reference the UI text elements.
    /// </summary>
    void Start()
    {
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();

        aSrc = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Adds to the score. The higher the level and the more lines, the higher the score.
    /// </summary>
    /// <param name="lines"></param>
    public void AddToScore(int lines)
    {
        if (currentLevel > 0)
        {
            currentScore += lines * currentLevel * 40;
        }
        else
        {
            currentScore += lines * 40;
        }

        scoreText.text = currentScore.ToString();

        aSrc.clip = clearLine;
        aSrc.Play();

        SetLevel();
    }

    /// <summary>
    /// Adds to the score. The higher the level and the more lines, the higher the score.
    /// </summary>
    /// <param name="lines"></param>
    public void SetLevel()
    {
        if (currentScore >= 1000)
        {
            currentLevel = 5;
            fallTime = 0.3f;
        }
        else if (currentScore >= 800)
        {
            currentLevel = 4;
            fallTime = 0.4f;
        }
        else if (currentScore >= 600)
        {
            currentLevel = 3;
            fallTime = 0.5f;
        }
        else if (currentScore >= 200)
        {
            currentLevel = 2;
            fallTime = 0.6f;
        }
        else if (currentScore >= 100)
        {
            currentLevel = 1;
            fallTime = 0.7f;
        }

        levelText.text = currentLevel.ToString();
    }
}
