using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSoundsVolume : MonoBehaviour
{
    private AudioSource _sfxAudio;

    private void Start()
    {
        _sfxAudio = GetComponent<AudioSource>();
    }

    public void SetVolume(float vol)
    {
        _sfxAudio.volume = vol;
    }
}
