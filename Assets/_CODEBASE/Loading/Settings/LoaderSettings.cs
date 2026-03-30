using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "My Assets/Loading/LoaderSettings")]
public class LoaderSettings : ScriptableObject
{
    [Tooltip("Forced wait time in load scene")]
    public FloatReference forcedWaitTime;
    [Tooltip("Fade time before and after loading")]
    public FloatReference secondsToFadeCamera;
    [Tooltip("Which load scene to use")]
    public string loadSceneName;
}
