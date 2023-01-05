using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings
{
    public static float MasterVolume
    {
        get;
        private set;
    }

    public static void Init()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 100));
    }

    public static void SetMasterVolume(float value)
    {
        AudioListener.volume = value / 100;
        MasterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
}
