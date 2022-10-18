namespace BrokenGears.Audio {
    using UnityEngine;

    public class Audioplayer : MonoBehaviour {
        [SerializeField] private AudioSource source;
        [SerializeField] private bool canOverridePlayWhilePlaying;

        public void Play() {
            if (!source.isPlaying || canOverridePlayWhilePlaying) {
                source.Play();
            }
        }

        public void Stop() {
            if (source.isPlaying) {
                source.Stop();
            }
        }
    }
}