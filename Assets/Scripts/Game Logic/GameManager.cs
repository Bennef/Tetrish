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

        public float fallTime = 0.8f;

        public bool gameIsOver = false;

        private SpawnTetromino spawner;

        private AudioSource aSrc;
        public AudioClip clearLine, gameOver, levelUp;

        /// <summary>
        /// Reference the UI text elements, the spawner and the audioSource.
        /// </summary>
        void Start()
        {
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            levelText = GameObject.Find("Level Text").GetComponent<Text>();
            gameOverText = GameObject.Find("Game Over Text").GetComponent<Text>();

            spawner = FindObjectOfType<SpawnTetromino>();

            aSrc = GetComponent<AudioSource>();
        }


        void Update()
        {
            if (gameIsOver && Input.anyKeyDown)
            {
                gameIsOver = false;
                currentScore = 0;
                scoreText.text = currentScore.ToString();
                currentLevel = 0;
                levelText.text = currentLevel.ToString();
                ClearGrid();
                gameOverText.enabled = false;
                spawner.NewTetromino();
            }
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

        /// <summary>
        /// Clear the grid of all blocks.
        /// </summary>
        public void ClearGrid()
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Tetromino");
            foreach (GameObject obj in blocks)
            {
                Destroy(obj);
            }
        }

        /// <summary>
        /// Game over occurs when the grid space is occupied by a block.
        /// </summary>
        public void GameOver()
        {
            aSrc.clip = gameOver;
            aSrc.Play();
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
    }
}