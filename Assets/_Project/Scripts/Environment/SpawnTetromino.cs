using UnityEngine;

namespace Scripts.Environment
{
    /// <summary>
    /// A script to handle the spawning of a new random tetromino.
    /// </summary>
    public class SpawnTetromino : MonoBehaviour
    {
        public GameObject[] _tetrominos;

        /// <summary>
        /// Spawn a random tetromino at the top of the screen.
        /// </summary>
        public void NewTetromino()
        {
            Instantiate(_tetrominos[Random.Range(0, _tetrominos.Length)], 
                transform.position, 
                Quaternion.identity);
        }
    }
}