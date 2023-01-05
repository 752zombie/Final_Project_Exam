using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundMenu : MonoBehaviour
{
    [SerializeField]
    private Slider MasterVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI MasterVolumeNumber;

    private void OnEnable()
    {
        MasterVolumeSlider.SetValueWithoutNotify(SoundSettings.MasterVolume);
        MasterVolumeNumber.text = SoundSettings.MasterVolume.ToString();
    }

    public void ChangeMasterVolume(float value)
    {
        MasterVolumeNumber.text = value.ToString();
        SoundSettings.SetMasterVolume(value);
    }

}
