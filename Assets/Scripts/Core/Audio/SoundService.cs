using UnityEngine;

namespace TestTask.Core.Audio
{
    public sealed class SoundService : ISoundService
    {
        private readonly AudioSource _audioSource;

        public SoundService(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void PlayOneShot(AudioClip clip, float volume, float pitch)
        {
            if (clip == null)
                return;

            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}
