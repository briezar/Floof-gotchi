using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class AudioObject : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public Action OnStop;

        public bool mute
        {
            get => _audioSource.mute;
            set => _audioSource.mute = value;
        }

        public void Play(AudioClip clip, bool loop, float volume, float delay)
        {
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.volume = volume;

            if (delay > 0)
            {
                _audioSource.PlayDelayed(delay);
            }
            else
            {
                _audioSource.Play();
            }


            if (!loop)
            {
                StartCoroutine(CheckStopRoutine());
            }


            IEnumerator CheckStopRoutine()
            {
                while (_audioSource.isPlaying)
                {
                    yield return null;
                }
                OnStop?.Invoke();
            }
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }


        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Unpause()
        {
            _audioSource.UnPause();
        }

        public void Stop()
        {
            StopAllCoroutines();
            _audioSource.Stop();
            OnStop?.Invoke();
        }
    }
}
