﻿using System;
using UnityEngine;

namespace Tetrish
{

    /// <summary>
    /// Handles the behaviour of the current active tetromino object. Checks for valid moves, if a line should be cleared, etc.
    /// </summary>
    public class TetrishBlock : MonoBehaviour
    {
        [SerializeField]
        InputHandler inputHandler;

        private static readonly int height = 20;
        private static readonly int width = 10;
        private Vector3 rotationPoint;
        private float previousTime;
        private float fallTime = 0.8f;
        private static Transform[,] grid = new Transform[width, height];
        private GameManager gameManager;
        private SpawnTetromino spawner;
        private SFXManager sFXManager;


        /// <summary>
        /// Set up references.
        /// </summary>
        void OnEnable()
        {
            sFXManager = FindObjectOfType<SFXManager>();
            gameManager = FindObjectOfType<GameManager>();
            spawner = FindObjectOfType<SpawnTetromino>();
            fallTime = gameManager.FallTime;
        }

        /// <summary>
        /// Handle the input every frame.
        /// </summary>
        void Update() => HandleInput();
        
        /// <summary>
        /// Handle any key presses and move the tetrominos if the move is valid.
        /// </summary>
        private void HandleInput()
        {
            if (inputHandler.GetLeftButtonDown())
            {
                MoveLeft();
            }
            else if (inputHandler.GetRightButtonDown())
            {
                MoveRight();
            }
            else if (inputHandler.GetRotateButtonDown())
            {
                Rotate();
            }

            if (Time.time - previousTime > (inputHandler.GetDownButtonDown() ? fallTime / 10 : fallTime))
            {
                MoveTetrominoDown();                
            }
        }

        private void MoveLeft()
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        private void MoveRight()
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        private void Rotate()
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
            sFXManager.PlaySound(sFXManager.Rotate);
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
                sFXManager.PlaySound(sFXManager.BlockLand);

                AddToGrid();
                CheckForLines();
                                
                if (GameOverCheck())
                {
                    gameManager.GameOver();
                }
                else
                {
                    spawner.NewTetromino();
                }
                this.enabled = false;
            }
            previousTime = Time.time;
        }

        /// <summary>
        /// Check to see if there are any completed lines.
        /// </summary>
        void CheckForLines()
        {
            int lineCounter = 0;
            for (int i = height - 1; i >= 0; i--)
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
            {
                gameManager.AddToScore(lineCounter);
            }
        }

        /// <summary>
        /// Helper function to check for completed lines.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        bool HasLine(int i)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Deletes the row of blocks if player gets 10 in a row.
        /// </summary>
        /// <param name="i"></param>
        void DeleteLine(int i)
        {
            for (int j = 0; j < width; j++)
            {
                Destroy(grid[j, i].gameObject);
                grid[j, i] = null;
            }
        }

        /// <summary>
        /// Move the rows down when player clears lines.
        /// </summary>
        /// <param name="i"></param>
        void RowDown(int i)
        {
            for (int y = i; y < height; y++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (grid[j, y] != null)
                    {
                        grid[j, y - 1] = grid[j, y];
                        grid[j, y] = null;
                        grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
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

                grid[roundedX, roundedY] = children;
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

                if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX, roundedY] != null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if there is a block already in the grid at the spawn location.
        /// </summary>
        /// /// <returns>True if block in the way, false if not.</returns>
        public bool GameOverCheck()
        {
            if (grid[4, 18] != null)
            {
                return true;
            }
            return false;
        }
    }
}