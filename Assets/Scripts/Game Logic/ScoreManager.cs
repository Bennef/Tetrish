using UnityEngine;

/// <summary>
/// Manages the current score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int currentLevel = 1;

    // Need a reference to the UI to show score to the player.

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Adds to the score. The higher the level and the more lines, the higher the score.
    /// </summary>
    /// <param name="lines"></param>
    public void AddToScore(int lines)
    {
        currentScore += lines * currentLevel * 40; // To do - determine the best formula for this.
    }

    /// <summary>
    /// Reset the score to 0.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
    }
}
