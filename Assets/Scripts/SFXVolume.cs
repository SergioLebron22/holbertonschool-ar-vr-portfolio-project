using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXVolume : MonoBehaviour
{
    public AudioMixer audioMixer;   
    public Slider slider;           
    public string exposedParam = "SoundVolume"; 

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(exposedParam, 0.75f);
        slider.value = savedVolume;
        SetVolume(savedVolume);
        
        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);

        PlayerPrefs.SetFloat(exposedParam, value);
    }
}
