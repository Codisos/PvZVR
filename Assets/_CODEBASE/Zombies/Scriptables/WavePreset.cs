using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Preset", menuName = "My Assets/Zombies/NewWavePreset")]
public class WavePreset : ScriptableObject
{
    [SerializeField] private List<int> wave;

    public List<int> Wave => wave;
}
