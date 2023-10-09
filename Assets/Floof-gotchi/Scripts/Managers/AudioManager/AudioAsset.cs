using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Floof.Audio
{
    // [CreateAssetMenu()]
    public class AudioAsset : ScriptableObject
    {
        [field: SerializeField] public MusicCollection MusicCollection { get; private set; }
        [field: SerializeField] public SoundCollection SoundCollection { get; private set; }

#if UNITY_EDITOR
        [Button]
        private void AutoSet()
        {
            var audioGUIDs = AssetDatabase.FindAssets($"t:{nameof(AudioClip)}", new[] { "Assets/Floof-gotchi/Audio" });

            var musicCollection = new MusicCollection();
            var soundCollection = new SoundCollection();

            foreach (var guid in audioGUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                var name = asset.name;


                var music = typeof(MusicCollection).GetField(name);
                var sound = typeof(SoundCollection).GetField(name);

                if (music == null && sound == null)
                {
                    Debug.LogWarning($"Field for [{name}] does not exist", asset);
                    continue;
                }

                music?.SetValueDirect(__makeref(musicCollection), asset);
                sound?.SetValueDirect(__makeref(soundCollection), asset);
            }

            MusicCollection = musicCollection;
            SoundCollection = soundCollection;

            AssetDatabase.Refresh();
        }
#endif
    }

    [Serializable]
    public struct MusicCollection
    {

    }

    [Serializable]
    public struct SoundCollection
    {


    }
}