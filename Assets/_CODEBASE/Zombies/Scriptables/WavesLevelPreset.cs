using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Waves Preset", menuName = "My Assets/Zombies/NewWavesLevelPreset")]
public class WavesLevelPreset : ScriptableObject
{
    [SerializeField] List <WavePreset> wavePresets;

    public List<WavePreset> WavePresets => wavePresets;
}
