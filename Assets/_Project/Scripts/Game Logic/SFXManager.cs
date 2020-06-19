using UnityEngine;

namespace Tetrish
{
    /// <summary>
    /// Handles the SFX in game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        private AudioSource aSrc;
        [SerializeField]  private AudioClip rotate, blockLand, clearLine, gameOver, levelUp;

        public AudioClip Rotate { get; internal set; }
        public AudioClip BlockLand { get; internal set; }
        public AudioClip ClearLine { get => clearLine; set => clearLine = value; }
        public AudioClip GameOver { get => gameOver; set => gameOver = value; }
        public AudioClip LevelUp { get => levelUp; set => levelUp = value; }

        // Use this for initialization
        void Start() => aSrc = GetComponent<AudioSource>();

        /// <summary>
        /// Assign the passed AudioClip and play it.
        /// </summary>
        public void PlaySound(AudioClip soundToPlay)
        {
            aSrc.clip = soundToPlay;
            aSrc.Play();
        }
    }
}