// 2022-04-16   Sean Hall       Created script, with event to play chainsaw revving when triggered
// 2022-04-17   Sean Hall       Added audio script, to play clips from inspector-loaded array

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSounds : MonoBehaviour
{
    bool canPlayAgain = true;

    [SerializeField] private AudioClip[] clips;
    private AudioClip clip;
    private AudioSource myAudioSource;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        EventManager.current.onWeaponTriggerEnter += PlayHitSound; // Subscribes to EventManager (Event triggered by PlayerWeaponHit)
    }

    private void PlayHitSound()
    {
        if (canPlayAgain)
        {
            canPlayAgain = false; // Makes sure the sound can't be played in back-to-back frames
            // Debug.Log("Sound should play now"); // Play the sound

            clip = clips[Random.Range(0, clips.Length)]; // Assigns a random clip to be played from array
            myAudioSource.PlayOneShot(clip); // Play audio clip

            Invoke("CanPlayAgain", 0.2f); // Slight delay before allowing another revv
        }
    }

    private void CanPlayAgain()
    {
        canPlayAgain = true;
    }

    private void Update()
    {
        myAudioSource.volume = PlayerPrefs.GetFloat("SFXVol");
    }
}
