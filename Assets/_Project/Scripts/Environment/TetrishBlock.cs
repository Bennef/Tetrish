using UnityEngine;
using Scripts.Core;
using Scripts.Audio;
using Scripts.Inputs;

namespace Scripts.Environment
{
    /// <summary>
    /// Handles the behaviour of the current active tetromino object. Checks for valid moves, if a line should be cleared, etc.
    /// </summary>
    public class TetrishBlock : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private static readonly int _height = 20;
        private static readonly int _width = 10;
        private Vector3 _rotationPoint;
        private float _previousTime;
        private float _fallTime = 0.8f;
        private static Transform[,] _grid = new Transform[_width, _height];
        private GameManager _gameManager;
        private SpawnTetromino _spawner;
        private SFXManager _sFXManager;

        /// <summary>
        /// Set up references.
        /// </summary>
        void OnEnable()
        {
            _sFXManager = FindObjectOfType<SFXManager>();
            _gameManager = FindObjectOfType<GameManager>();
            _spawner = FindObjectOfType<SpawnTetromino>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _fallTime = _gameManager.FallTime;
        }

        /// <summary>
        /// Handle the input every frame.
        /// </summary>
        void Update()
        {
            if (_inputHandler.GetLeftButtonDown())
                MoveLeft();
            else if (_inputHandler.GetRightButtonDown())
                MoveRight();
            else if (_inputHandler.GetRotateButtonDown())
                Rotate();

            if (Time.time - _previousTime > (_inputHandler.GetDownButtonDown() ? _fallTime / 10 : _fallTime))
                MoveTetrominoDown();
        }
            
        private void MoveLeft()
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }

        private void MoveRight()
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }

        private void Rotate()
        {
            transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, 1), -90);
            _sFXManager.PlaySound(_sFXManager.Rotate);
        }

        /// <summary>
        /// Move tetromino down one gris square and check if the move is valid. 
        /// </summary>
        public void MoveTetrominoDown()
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                _sFXManager.PlaySound(_sFXManager.BlockLand);

                AddToGrid();
                CheckForLines();
                                
                if (GameOverCheck())
                    _gameManager.GameOver();
                else
                    _spawner.NewTetromino();
                this.enabled = false;
            }
            _previousTime = Time.time;
        }

        /// <summary>
        /// Check to see if there are any completed lines.
        /// </summary>
        void CheckForLines()
        {
            int lineCounter = 0;
            for (int i = _height - 1; i >= 0; i--)
            {
                if (HasLine(i))
                {
                    DeleteLine(i);
                    RowDown(i);
                    lineCounter++;
                }
            }

            // If we have at least one line, update the score.
            if (lineCounter > 0)
                _gameManager.AddToScore(lineCounter);
        }

        /// <summary>
        /// Helper function to check for completed lines.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        bool HasLine(int i)
        {
            for (int j = 0; j < _width; j++)
            {
                if (_grid[j, i] == null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes the row of blocks if player gets 10 in a row.
        /// </summary>
        /// <param name="i"></param>
        void DeleteLine(int i)
        {
            for (int j = 0; j < _width; j++)
            {
                Destroy(_grid[j, i].gameObject);
                _grid[j, i] = null;
            }
        }

        /// <summary>
        /// Move the rows down when player clears lines.
        /// </summary>
        /// <param name="i"></param>
        void RowDown(int i)
        {
            for (int y = i; y < _height; y++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (_grid[j, y] != null)
                    {
                        _grid[j, y - 1] = _grid[j, y];
                        _grid[j, y] = null;
                        _grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Adds all the cubes in every tetromino to the 2D grid so we can check the position of each one.
        /// </summary>
        void AddToGrid()
        {
            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);
                _grid[roundedX, roundedY] = children;
            }
        }

        /// <summary>
        /// Checks to see if the move is valid. If move is not valid we reverse the move.
        /// </summary>
        /// <returns>True if move is valid, false if not.</returns>
        bool ValidMove()
        {
            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                if (roundedX < 0 || 
                    roundedX >= _width || 
                    roundedY < 0 || 
                    roundedY >= _height || 
                    _grid[roundedX, roundedY] != null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if there is a block already in the grid at the spawn location.
        /// </summary>
        /// /// <returns>True if block in the way, false if not.</returns>
        public bool GameOverCheck()
        {
            if (_grid[4, 18] != null)
                return true;
            return false;
        }
    }
}