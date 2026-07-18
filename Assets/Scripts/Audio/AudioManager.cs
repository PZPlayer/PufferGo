using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PufferGo.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager MANAGER;

        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _meoldyieAudioSource;
        [SerializeField] private AudioSource _repeatedAuidoSource;
        [SerializeField] private AudioSource _importantAuidoSource;

        private void Awake()
        {
            if (MANAGER != null)
            {
                Destroy(gameObject);
            }

            MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayOneTime(AudioClip clip)
        {
            _sfxAudioSource.PlayOneShot(clip);
        }

        public void PlayMelodie(AudioClip clip)
        {
            _meoldyieAudioSource.clip = clip;
            _meoldyieAudioSource.Play();
        }

        public void PlayAudioRepeatedly(AudioClip audioSource)
        {
            _repeatedAuidoSource.clip = audioSource;
            if (!_repeatedAuidoSource.isPlaying)
                _repeatedAuidoSource.Play();
        }

        public void PlayImportantOneTime(AudioClip clip)
        {
            _importantAuidoSource.PlayOneShot(clip);
        }
    }
}