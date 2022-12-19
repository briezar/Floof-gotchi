using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    private AudioSource _source;
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

}
