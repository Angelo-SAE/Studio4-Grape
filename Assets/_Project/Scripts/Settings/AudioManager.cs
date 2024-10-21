using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private FloatObject masterVolume;

    [Header("Audio Variables")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI")]
    [SerializeField] private Slider masterVolumeSlider;

    private void Start() //will be moved to loading in main menu
    {
        masterVolume.value = 0;
        masterVolumeSlider.value = masterVolume.value;
        audioMixer.SetFloat("MasterVolume", masterVolume.value);
    }

    public void OnChangeMasterVolume()
    {
        masterVolume.value = masterVolumeSlider.value;
        if(masterVolume.value <= -39f)
        {
            masterVolume.value = -80;
            audioMixer.SetFloat("MasterVolume", masterVolume.value);
        } else {
            audioMixer.SetFloat("MasterVolume", masterVolume.value);
        }

    }
}
