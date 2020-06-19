using UnityEngine;

namespace Tetrish
{
    /// <summary>
    /// A script to handle the spawning of a new random tetromino.
    /// </summary>
    public class SpawnTetromino : MonoBehaviour
    {
        public GameObject[] Tetrominos;


        /// <summary>
        /// Spawn a random tetromino at the top of the screen.
        /// </summary>
        public void NewTetromino()
        {
            Instantiate(Tetrominos[Random.Range(0, Tetrominos.Length)], transform.position, Quaternion.identity);
        }
    }
}