using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    [SerializeField]
    private Toggle VSyncToggle;

    private Resolution[] supportedResolutions;
    private List<Resolution> filteredResolutions;

    private Resolution resolutionToApply;
    private bool VSyncToApply;

    private void OnEnable()
    {
        resolutionToApply = Screen.currentResolution;
        VSyncToApply = GraphicsSettings.VSyncOn;
        FillResolutionDropdown();
        VSyncToggle.SetIsOnWithoutNotify(VSyncToApply);
    }

    private void FillResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        
        supportedResolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>(System.Array.FindAll(supportedResolutions, x => x.refreshRate == Screen.currentResolution.refreshRate));
        filteredResolutions.Reverse();
        int selectedValue = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            options.Add(filteredResolutions[i].width + "x" + filteredResolutions[i].height + " @ " + filteredResolutions[i].refreshRate + "Hz");
            
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                selectedValue = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(selectedValue);
    }

    public void SetResolution(int index)
    {
        resolutionToApply = filteredResolutions[index];
    }

    public void SetVSync(bool VSyncOn)
    {
        VSyncToApply = VSyncOn;
    }

    public void ApplySettings()
    {
        Screen.SetResolution(resolutionToApply.width, resolutionToApply.height, true);
        GraphicsSettings.SetVSyncOn(VSyncToApply);
    }
}

