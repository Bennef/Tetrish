using UnityEngine;

namespace Tetrish
{
    /// <summary>
    /// Handles the SFX in game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        private AudioSource aSrc;
        [SerializeField]  public AudioClip rotate, blockLand, clearLine, gameOver, levelUp;

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