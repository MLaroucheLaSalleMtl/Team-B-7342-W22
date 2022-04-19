// 2022-04-17   Sean Hall       Created script with event listeners and play/stop methods

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawParticles : MonoBehaviour
{
    private ParticleSystem particle;    

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop(); // Prevents particles playing as default state
        EventManager.current.onWeaponTriggerEnter += StartBloodStream;
        EventManager.current.onWeaponTriggerExit += StopBloodStream;
    }    

    private void StartBloodStream()
    {
        particle.Play();
    }

    private void StopBloodStream()
    {
        particle.Stop();
    }
}
