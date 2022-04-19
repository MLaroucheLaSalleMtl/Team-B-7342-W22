using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager current;

    public AudioSource music;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    public void UpdateMusic(AudioClip newTrack)
    {
        music.Stop();
        music.clip = newTrack;
        music.Play();
    }

    private void Update()
    {
        music.volume = PlayerPrefs.GetFloat("MusicVol");
    }
}
