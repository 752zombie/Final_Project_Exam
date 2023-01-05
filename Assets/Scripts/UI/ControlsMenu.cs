using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider CameraXSlider;

    [SerializeField]
    private TextMeshProUGUI CameraXNumber;
 
    private void OnEnable()
    {
        CameraXSlider.SetValueWithoutNotify(ControlSettings.CameraSensitivityX);
        CameraXNumber.text = ControlSettings.CameraSensitivityX.ToString();
    }

    public void SetCameraSensitivityX(float value)
    {
        CameraXNumber.text = value.ToString();
        ControlSettings.SetCameraSensitivityX(value);
    }
}
