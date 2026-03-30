using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private FloatReference timeBetweenWaves;

    [Header("SPAWNING")]
    [SerializeField] private Transform[] startPoints;
    [SerializeField] private Transform[] endPoints;

    [Header("SETUP")]
    [SerializeField]CurrentWavesSetting wavesInLevelPreset;
    [SerializeField] private int numberOfWaves = 1;
    [SerializeField] private List<List<int>> waves = new List<List<int>>();     //type of zombies in each wave
    [SerializeField] private List<int> numOfZombiesInWaves = new List<int>();   //number of zombies for each wave

    [Header("PARTICLES")]
    [SerializeField] private GameObject digUpParicles;

    public static event Action<bool[]> RequestFlagStatus;
    public static event Action<List<float>> RequestFlagPositions;
    public static event Action<int> UpdateCurrentWave;

    private List<int> spawnPositions;
    private List<int> zombiesBeforFlagList = new List<int>();
    private List<float> flagPosList = new List<float>();
    private bool[] flagStatus = {false, false, false, false, false, false, false, false, false, false};
    private int _currentWave = 0;

    private int zombiesAlive; //control for next wave spawning
    private int numOfAllZombies = 0;


    private void Awake()
    {
        Zombie.ZombieKilled += ZombieDeadCheck;
    }

    void Start()
    {
        if (wavesInLevelPreset != null)
        {
            numOfZombiesInWaves.Clear();
            waves.Clear();
            numberOfWaves = wavesInLevelPreset.currentPreset.WavePresets.Count;

            //migrate data from preset to active local list
            for (int i = 0; i < numberOfWaves; i++)
            {
                numOfZombiesInWaves.Add(wavesInLevelPreset.currentPreset.WavePresets[i].Wave.Count);
                waves.Add(wavesInLevelPreset.currentPreset.WavePresets[i].Wave);

                if(numOfZombiesInWaves[i] < 10)
                {
                    flagStatus[i] = false;
                    numOfAllZombies += numOfZombiesInWaves[i];
                }
                else
                {
                    flagStatus[i] = true;
                    numOfAllZombies += numOfZombiesInWaves[i];
                }
            }
        }
        else if (wavesInLevelPreset == null)
        {
            //generate num of zombies for each wave if not set in menu
            if (numOfZombiesInWaves.Count == 0)
            {
                for (int i = 0; i < numberOfWaves; i++)
                {
                    numOfZombiesInWaves.Add(UnityEngine.Random.Range(1, 11));
                    numOfAllZombies += numOfZombiesInWaves[i];
                }
            }

            //generate random waves if not set in menu
            if (waves.Count == 0)
            {
                //for loop(num of waves)
                for (int i = 0; i < numberOfWaves; i++)
                {

                    List<int> oneTempList = new List<int>();

                    for (int x = 0; x < numOfZombiesInWaves[i]; x++)
                    {
                        oneTempList.Add(UnityEngine.Random.Range(1, 4));
                    }

                    waves.Add(oneTempList);

                }
            }
        }

        RequestFlagStatus?.Invoke(flagStatus);
        RequestFlagPositions?.Invoke(CalculateFloatFlagPositions());

        StartSpawning();    //delete here, startSpawning is on level started listener
    }

    List<float> CalculateFloatFlagPositions()
    {
        int zombiesBeforFlag = 0;


        for (int i = 0; i < numOfZombiesInWaves.Count; i++)
        {
            if (numOfZombiesInWaves[i] < 10)
            {
                zombiesBeforFlag += numOfZombiesInWaves[i];
            }
            else
            {
                zombiesBeforFlagList.Add(zombiesBeforFlag);
                flagPosList.Add((float)zombiesBeforFlagList[zombiesBeforFlagList.Count - 1] / numOfAllZombies);
                zombiesBeforFlag += numOfZombiesInWaves[i];
            }           
        }

        return flagPosList;
    }

    public List<int> GetAllWaves()
    {
        return numOfZombiesInWaves;
    }

    public int GetCurrentWave()
    {
        return _currentWave;
    }

    void ZombieDeadCheck()
    {
        zombiesAlive--;

        if(zombiesAlive == 1)   //change to 0??? bylo by to lepe rozdelene pak v ui 
        {
            CanSpawnNextWave();
        }

    }


    List<int> GenerateSpawnPositions(int currentWave)    //for one wave
    {
        List<int> numbers = new List<int>();

        // Fill the list with random numbers
        for (int i = 0; i < numOfZombiesInWaves[currentWave]; i++)
        {
            numbers.Add(UnityEngine.Random.Range(0, startPoints.Length));
        }

        // Ensure no two adjacent numbers are the same
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] == numbers[i - 1])
            {
                // Swap with a non-adjacent number
                int swapIndex = (i + 1) % numbers.Count;
                int temp = numbers[i];
                numbers[i] = numbers[swapIndex];
                numbers[swapIndex] = temp;
            }
        }

        return numbers;
    }

    public void StartSpawning() //init spawning at the start of the game (gameEvent)
    {

         SpawnWave(0);

    }

    void CanSpawnNextWave()
    {
        if (_currentWave < numberOfWaves)
        {
            StartCoroutine(WaitForNextWave(_currentWave));
        }
    }

    void SpawnWave(int wave)
    {

        spawnPositions = GenerateSpawnPositions(wave);  //generate spawn positions for current wave

        for (int zombieCount = 0; zombieCount < numOfZombiesInWaves[wave]; zombieCount++) //spawn zombies for wave
        {
            //if xxx same spawn or not
            if (zombieCount == 0)
            {
                SpawnZombie(waves[wave][zombieCount], spawnPositions[zombieCount]);
            }
            else
            {
                StartCoroutine(WaitForNextZombie(UnityEngine.Random.Range(2f, 15f), waves[wave][zombieCount], spawnPositions[zombieCount]));
            }
        }

        _currentWave++;
        UpdateCurrentWave?.Invoke(_currentWave);
    }

    void SpawnZombie(int type, int startPos)
    {
        GameObject newZombie = null;

        switch (type)
        {
            case 1:
                newZombie = ZombiePool.Instance.GetPooledObject();
                break;
            case 2:
                //light armored zombiePool
                newZombie = HeadsetZombiePool.Instance.GetPooledObject();
                break;
            case 3:
                //mid armored zombiePool
                newZombie = ConeZombiePool.Instance.GetPooledObject();
                break;
            case 4:
                //heavy armored zombiePool
                newZombie = ArmorZombiePool.Instance.GetPooledObject();
                break;
        }


        SpawnParticles(startPoints[startPos]);

        newZombie.transform.position = startPoints[startPos].position;

        newZombie.SetActive(true);

        newZombie.GetComponent<Zombie>().OnSpawn(endPoints[startPos].position);

        zombiesAlive++;

    }

    IEnumerator WaitForNextWave(int wave)
    {
        yield return new WaitForSeconds(timeBetweenWaves.Value);    //+last zombie spawn
        SpawnWave(wave);
    }

    IEnumerator WaitForNextZombie(float time, int type, int startpos)
    {
        
        yield return new WaitForSeconds(time);
        SpawnZombie(type, startpos);

    }

    void SpawnParticles(Transform orig)
    {
        if (digUpParicles != null)
        {
            Instantiate(digUpParicles, orig);
        }
    }
}
