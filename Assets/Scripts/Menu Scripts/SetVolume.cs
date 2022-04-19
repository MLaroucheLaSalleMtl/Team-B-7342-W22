using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private string nameParam = null;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        float vol = PlayerPrefs.GetFloat(nameParam, 0.5f);
        slider.value = vol;
    }

    public void SetVol(float vol)
    {
        PlayerPrefs.SetFloat(nameParam, vol);
    }
}
