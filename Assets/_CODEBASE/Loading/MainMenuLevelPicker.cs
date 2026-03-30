using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelPicker : MonoBehaviour
{

    CurrentWavesSetting currentWaveSetting;
    Object[] levelPresets;

    // Start is called before the first frame update
    void Start()
    {
        currentWaveSetting = FindSettings();
        levelPresets = FindLevelPresets();
    }


    public void PickLevel(int index)
    {
        //set level preset
        currentWaveSetting.currentPreset = levelPresets[index] as WavesLevelPreset;

        //get level loader and start loading
        LoadManager.Instance.LoadLevel("SampleScene");
    }

    CurrentWavesSetting FindSettings()
    {
        var settings = Resources.Load("CurrentWaveLevel_DO_NOT_DELETE");

        if (settings != null)
        {
            return settings as CurrentWavesSetting;
        }
        
        return null;
    }

    //find level preset
    Object[] FindLevelPresets()
    {
        var preset = Resources.FindObjectsOfTypeAll(typeof(WavesLevelPreset));

        if (preset != null)
        {

            return preset;

        }
        
        return null;
    }
}
