using System.Collections;
using UnityEngine;

namespace Tetrish
{
    /// <summary>
    /// Manages the Game events.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private int currentScore = 0;
        private int currentLevel = 0;
        private bool gameIsOver = false;
        private SpawnTetromino spawner;
        private SFXManager sFXManager;
        private UIManager uIManager;
        private InputHandler inputHandler;

        public float FallTime { get; set; } = 0.8f;

        /// <summary>
        /// Reference the UI text elements, the spawner and the audioSource.
        /// </summary>
        void Start()
        {
            spawner = FindObjectOfType<SpawnTetromino>();
            sFXManager = FindObjectOfType<SFXManager>();
            uIManager = FindObjectOfType<UIManager>();
            inputHandler = FindObjectOfType<InputHandler>();
            spawner.NewTetromino();
        }

        /// <summary>
        /// Check for input when the game is over.
        /// </summary>
        void Update()
        {
            if (gameIsOver && inputHandler.GetAnyKeyDown())
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
            uIManager.SetText(uIManager.ScoreText, currentScore.ToString());            
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
                SetLevel(5);
                FallTime = 0.3f;
            }
            else if (currentScore >= 800)
            {
                SetLevel(4);
                FallTime = 0.4f;
            }
            else if (currentScore >= 600)
            {
                SetLevel(3);
                FallTime = 0.5f;
            }
            else if (currentScore >= 200)
            {
                SetLevel(2);
                FallTime = 0.6f;
            }
            else if (currentScore >= 100)
            {
                SetLevel(1);
                FallTime = 0.7f;
            }
            uIManager.SetText(uIManager.LevelText, currentLevel.ToString());
        }

        private void SetLevel(int levelToSet)
        {
            currentLevel = levelToSet;
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
            uIManager.ShowText(uIManager.GameOverText);
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
            uIManager.HideText(uIManager.GameOverText);
            uIManager.SetText(uIManager.ScoreText, "0");
            uIManager.SetText(uIManager.LevelText, "0");
            ClearGrid();
            spawner.NewTetromino();
        }
    }
}