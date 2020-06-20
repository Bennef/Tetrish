using Scripts.Audio;
using Scripts.Environment;
using System.Collections;
using Scripts.Inputs;
using UnityEngine;

namespace Scripts.Core
{
    /// <summary>
    /// Manages the Game events.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private int _currentScore = 0;
        private int _currentLevel = 0;
        private bool _gameIsOver = false;
        private SpawnTetromino _spawner;
        private SFXManager _sFXManager;
        private UIManager _uIManager;
        private InputHandler _inputHandler;

        public float FallTime { get; set; } = 0.8f;

        /// <summary>
        /// Reference the UI text elements, the spawner and the audioSource.
        /// </summary>
        void Start()
        {
            _spawner = FindObjectOfType<SpawnTetromino>();
            _sFXManager = FindObjectOfType<SFXManager>();
            _uIManager = FindObjectOfType<UIManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _spawner.NewTetromino();
        }

        /// <summary>
        /// Check for input when the game is over.
        /// </summary>
        void Update()
        {
            if (_gameIsOver && _inputHandler.GetAnyKeyDown())
                ResetGame();
        }

        /// <summary>
        /// Adds to the score. The higher the level and the more lines, the higher the score.
        /// </summary>
        /// <param name="lines"></param>
        public void AddToScore(int lines)
        {
            _currentScore = _currentLevel > 0 ? _currentScore += lines * _currentLevel * 40 : _currentScore += lines * 40;
            _uIManager.SetText(_uIManager.ScoreText, _currentScore.ToString());
            _sFXManager.PlaySound(_sFXManager.ClearLine);
            SetLevel();
        }

        /// <summary>
        /// Adds to the score. The higher the level and the more lines, the higher the score.
        /// </summary>
        /// <param name="lines"></param>
        public void SetLevel()
        {
            if (_currentScore >= 1000)
            {
                SetLevel(5);
                FallTime = 0.3f;
            }
            else if (_currentScore >= 800)
            {
                SetLevel(4);
                FallTime = 0.4f;
            }
            else if (_currentScore >= 600)
            {
                SetLevel(3);
                FallTime = 0.5f;
            }
            else if (_currentScore >= 200)
            {
                SetLevel(2);
                FallTime = 0.6f;
            }
            else if (_currentScore >= 100)
            {
                SetLevel(1);
                FallTime = 0.7f;
            }
            _uIManager.SetText(_uIManager.LevelText, _currentLevel.ToString());
        }

        private void SetLevel(int levelToSet) => _currentLevel = levelToSet;

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
            _sFXManager.PlaySound(_sFXManager.GameOver);
            _uIManager.ShowText(_uIManager.GameOverText);
            StartCoroutine(GameOverDelay());
        }

        /// <summary>
        /// Wait 2 seconds so player does not start a new game too quickly.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GameOverDelay()
        {
            yield return new WaitForSeconds(2);
            _gameIsOver = true;
        }

        /// <summary>
        /// Reset all the values and text and spawn a new Tetromino to restart the game.
        /// </summary>
        public void ResetGame()
        {
            _gameIsOver = false;
            _uIManager.HideText(_uIManager.GameOverText);
            _uIManager.SetText(_uIManager.ScoreText, "0");
            _uIManager.SetText(_uIManager.LevelText, "0");
            ClearGrid();
            _spawner.NewTetromino();
        }
    }
}