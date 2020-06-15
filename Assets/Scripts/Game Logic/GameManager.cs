using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tetrish
{

    /// <summary>
    /// Manages the current score.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private int currentScore = 0;
        private int currentLevel = 0;

        private Text scoreText;
        private Text levelText;
        private Text gameOverText;

        private bool gameIsOver = false;

        private SpawnTetromino spawner;

        public float fallTime = 0.8f;

        private SFXManager sFXManager;

        /// <summary>
        /// Reference the UI text elements, the spawner and the audioSource.
        /// </summary>
        void Start()
        {
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            levelText = GameObject.Find("Level Text").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Text").GetComponent<Text>();

            spawner = FindObjectOfType<SpawnTetromino>();

            sFXManager = FindObjectOfType<SFXManager>();
        }

        /// <summary>
        /// Check for input when the game is over.
        /// </summary>
        void Update()
        {
            if (gameIsOver && Input.anyKeyDown)
            {
                ResetGame();
            }
        }

        /// <summary>
        /// Adds to the score. The higher the level and the more lines, the higher the score.
        /// </summary>
        /// <param name="lines"></param>
        public void AddToScore(int lines)
        {
            currentScore = currentLevel > 0 ? currentScore += lines * currentLevel * 40 : currentScore += lines * 40;

            SetScoreText(currentScore);

            sFXManager.PlaySound(sFXManager.clearLine);

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

            SetLevelText(currentLevel);
        }

        /// <summary>
        /// Clear the grid of all blocks.
        /// </summary>
        public void ClearGrid()
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Tetromino");
            foreach (GameObject block in blocks)
            {
                Destroy(block);
            }
        }

        /// <summary>
        /// Game over occurs when the grid space is occupied by a block.
        /// </summary>
        public void GameOver()
        {
            sFXManager.PlaySound(sFXManager.gameOver);
            gameOverText.enabled = true;
            StartCoroutine(GameOverDelay());
        }

        /// <summary>
        /// Wait 2 seconds so player does not start a new game too quickly.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GameOverDelay()
        {
            yield return new WaitForSeconds(2);
            gameIsOver = true;
        }

        /// <summary>
        /// Reset all the values and text and spawn a new Tetromino to restart the game.
        /// </summary>
        public void ResetGame()
        {
            gameIsOver = false;
            gameOverText.enabled = false;
            SetScoreText(0);
            SetLevelText(0);
            ClearGrid();
            spawner.NewTetromino();
        }

        /// <summary>
        /// Update the Level UI value to feed back to the player.
        /// </summary>
        public void SetLevelText(int level)
        {
            levelText.text = level.ToString();
        }

        /// <summary>
        /// Update the Score UI value to feed back to the player.
        /// </summary>
        public void SetScoreText(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}