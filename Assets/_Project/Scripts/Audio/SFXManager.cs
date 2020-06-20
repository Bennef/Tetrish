using UnityEngine;

namespace Scripts.Audio
{
    /// <summary>
    /// Handles the SFX in game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        private AudioSource aSrc;
        [SerializeField] private AudioClip _rotate, _blockLand, _clearLine, _gameOver, _levelUp;

        public AudioClip Rotate { get => _rotate; set => _rotate = value; }
        public AudioClip BlockLand { get => _blockLand; set => _blockLand = value; }
        public AudioClip ClearLine { get => _clearLine; set => _clearLine = value; }
        public AudioClip GameOver { get => _gameOver; set => _gameOver = value; }
        public AudioClip LevelUp { get => _levelUp; set => _levelUp = value; }

        void Start() => aSrc = GetComponent<AudioSource>();

        /// <summary>
        /// Assign the passed AudioClip and play it.
        /// </summary>
        public void PlaySound(AudioClip soundToPlay) => aSrc.PlayOneShot(soundToPlay);
    }
}