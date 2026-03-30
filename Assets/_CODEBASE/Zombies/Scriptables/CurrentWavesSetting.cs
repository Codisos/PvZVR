using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Current Waves Setting", menuName = "My Assets/Zombies/CurrentWavesSetting")]
public class CurrentWavesSetting : ScriptableObject
{
    public WavesLevelPreset currentPreset;
}
