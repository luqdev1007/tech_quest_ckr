using UnityEngine;

namespace TestTask.Core.Audio
{
    public interface ISoundService
    {
        void PlayOneShot(AudioClip clip, float volume, float pitch);
    }
}
