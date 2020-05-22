using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the current score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int currentLevel = 1;

    private Text scoreText;
    private Text levelText;
    
    // Use this for initialization
    void Start()
    {
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
    }

    /// <summary>
    /// Adds to the score. The higher the level and the more lines, the higher the score.
    /// </summary>
    /// <param name="lines"></param>
    public void AddToScore(int lines)
    {
        currentScore += lines * currentLevel * 40; // To do - determine the best formula for this.
        scoreText.text = currentScore.ToString();
    }

    /// <summary>
    /// Adds to the score. The higher the level and the more lines, the higher the score.
    /// </summary>
    /// <param name="lines"></param>
    public void AddToLevel()
    {
        currentLevel += 1;
        levelText.text = currentLevel.ToString();
    }

    /// <summary>
    /// Reset the score to 0.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
    }
}
