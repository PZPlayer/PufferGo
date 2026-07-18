using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PufferGo.Audio
{
    public class AudioUser : MonoBehaviour
    {
        public void PlayAudioOnce(AudioClip audioSource)
        {
            AudioManager.MANAGER.PlayOneTime(audioSource);
        }

        public void PlayImportantAudioOnce(AudioClip audioSource)
        {
            AudioManager.MANAGER.PlayImportantOneTime(audioSource);
        }

        public void ChangeAudioMelodie(AudioClip audioSource)
        {
            AudioManager.MANAGER.PlayMelodie(audioSource);
        }

        public void PlayAudioRepeatedly(AudioClip audioSource)
        {
            AudioManager.MANAGER.PlayAudioRepeatedly(audioSource);
        }
    }
}