using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSettings
{
    /// <summary>
    /// Camera sensitivity on x-axis as represented in UI
    /// </summary>
    public static float CameraSensitivityX
    {
        get;
        private set;
    }

    /// <summary>
    /// Camera sensitivity on x-axis relative to normal
    /// </summary>
    public static float CameraSensitivityXNormal
    {
        get;
        private set;
    }

    public static void Init()
    {
        SetCameraSensitivityX(PlayerPrefs.GetFloat("CameraSensitivityX", 5));
    }

    public static void SetCameraSensitivityX(float value)
    {
        CameraSensitivityX = value;
        CameraSensitivityXNormal = value / 5;
        PlayerPrefs.SetFloat("CameraSensitivityX", value);
    }
}
