using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettings
{
    public static bool VSyncOn
    {
        get;
        private set;
    }

    public static int VSyncCount
    {
        get;
        private set;
    }

    public static void Init()
    {
        SetVSyncCount(PlayerPrefs.GetInt("VSyncCount", QualitySettings.vSyncCount));
    }


    private static void SetVSyncCount(int VSyncCount)
    {
        GraphicsSettings.VSyncCount = VSyncCount;
        QualitySettings.vSyncCount = VSyncCount;
        PlayerPrefs.SetInt("VSyncCount", VSyncCount);
        VSyncOn = VSyncCount > 0;
    }

    public static void SetVSyncOn(bool VSyncOn)
    {
        GraphicsSettings.VSyncOn = VSyncOn;
        int VSyncCount = VSyncOn ? 1 : 0;
        GraphicsSettings.VSyncCount = VSyncCount;
        QualitySettings.vSyncCount = VSyncCount;
        PlayerPrefs.SetInt("VSyncCount", VSyncCount);
    }
}
