using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieWaveSlider : MonoBehaviour
{

    [SerializeField] WaveSystem waveSys;

    private Slider slider;
    private List<int> zombies;
    private int zombieCount = 0;
    private int deadZombieCount = 0;
    private int _currentWave =0;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        zombies = waveSys.GetAllWaves();

        Zombie.ZombieKilled += ZombieKilledNotification;
        WaveSystem.UpdateCurrentWave += UpdateCurrentWaveNumber;

        for( int i = 0; i < zombies.Count ; i++)
        {
            zombieCount += zombies[i];
        }
    }

    void ZombieKilledNotification()
    {
        deadZombieCount++;

       // _currentWave = waveSys.GetCurrentWave();

   
        float sliderPos = slider.value + (1f / zombieCount);

        slider.value = sliderPos;
    }

    void UpdateCurrentWaveNumber(int c)
    {
        _currentWave = c;
    }

    void CalculateCurrentWave()
    {

    }

    
}
