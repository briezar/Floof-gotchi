using System.Collections;
using System.Collections.Generic;
using Floof.Audio;
using UnityEngine;

namespace Floof
{
    public class AudioManager : MonoBehaviour, IConstructable
    {
        [SerializeField] private AudioObject _audioObjSample;

        [NaughtyAttributes.Expandable]
        [SerializeField] private AudioAsset _audioAsset;

        private ObjectPool<AudioObject> _soundPool;
        private AudioObject _musicObj;

        private static AudioManager _instance;
        public static MusicCollection Music => _instance._audioAsset.MusicCollection;
        public static SoundCollection Sound => _instance._audioAsset.SoundCollection;

        public const string MUSIC_VOLUME = "MUSIC_VOLUME";
        public const string SOUND_VOLUME = "SOUND_VOLUME";
        public const string MUTE_SOUND = "MUTE_SOUND";
        public const string MUTE_MUSIC = "MUTE_MUSIC";
        public const string ALLOW_VIBRATION = "ALLOW_VIBRATION";

        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f);
            set
            {
                var volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
                _instance._musicObj.SetVolume(volume);
            }
        }

        public static float SoundVolume
        {
            get => PlayerPrefs.GetFloat(SOUND_VOLUME, 1f);
            set
            {
                var volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(SOUND_VOLUME, volume);
                foreach (var audio in _instance._soundPool.ActiveElements)
                {
                    audio.SetVolume(volume);
                }
            }
        }

        public static bool AllowVibration
        {
            get => PlayerPrefsExt.GetBool(ALLOW_VIBRATION, true);
            set => PlayerPrefsExt.SetBool(ALLOW_VIBRATION, value);
        }

        public static bool MuteMusic
        {
            get => PlayerPrefsExt.GetBool(MUTE_MUSIC);
            set
            {
                PlayerPrefsExt.SetBool(MUTE_MUSIC, value);
                _instance._musicObj.mute = value;
            }
        }

        public static bool MuteSound
        {
            get => PlayerPrefsExt.GetBool(MUTE_SOUND);
            set
            {
                PlayerPrefsExt.SetBool(MUTE_SOUND, value);
                foreach (var audio in _instance._soundPool.ActiveElements)
                {
                    audio.mute = value;
                }
            }
        }

        void IConstructable.Construct()
        {
            if (_instance != null)
            {
                return;
            }

            _instance = this;
            _soundPool = new ObjectPool<AudioObject>(_audioObjSample);
            _musicObj = Instantiate(_audioObjSample, _audioObjSample.transform.parent);
            _musicObj.gameObject.SetActive(true);
            _musicObj.name = "MusicSource";
        }

        public static void PlayMusic(AudioClip music, float delay = 0)
        {
            _instance._musicObj.Play(music, true, MusicVolume, delay);
            _instance._musicObj.mute = MuteMusic;
        }

        public static void StopMusic()
        {
            _instance._musicObj.Stop();
        }

        public static void PauseMusic()
        {
            _instance._musicObj.Pause();
        }

        public static void UnpauseMusic()
        {
            _instance._musicObj.Unpause();
        }

        public static AudioObject PlaySound(AudioClip sfx, float delay = 0)
        {
            var soundObj = _instance._soundPool.Get();
            soundObj.Play(sfx, false, SoundVolume, delay);
            soundObj.OnStop = () =>
            {
                _instance._soundPool.Store(soundObj);
            };
            soundObj.mute = MuteSound;

            return soundObj;
        }

        public static void Vibrate()
        {
            if (AllowVibration)
            {
                Handheld.Vibrate();
            }
        }
    }
}