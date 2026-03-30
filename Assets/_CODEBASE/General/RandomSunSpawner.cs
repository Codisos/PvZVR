using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSunSpawner : MonoBehaviour
{
    [SerializeField]Transform limitA;
    [SerializeField]Transform limitB;
    [Tooltip("Min max value of spawn rate(pause between spawns in seconds)")][SerializeField] Vector2 sunSpawnRate;

    Coroutine sunSpawnCorutine;
    WaitForSeconds definedSunSpawnRate;

    bool gameRunning = true;

    private void Awake()
    {
        LevelActions.OnGameOver += OnGameOver;
    }

    private void Start()
    {
        sunSpawnCorutine = StartCoroutine(SunSpawnerCorutine());
    }

    IEnumerator SunSpawnerCorutine()
    {

        while (gameRunning)
        {
            yield return new WaitForSeconds(Random.Range(sunSpawnRate.x, sunSpawnRate.y));

            SpawnSun();
        }

    }

    void OnGameOver()
    {
        if(sunSpawnCorutine!=null) StopCoroutine(sunSpawnCorutine);
    }

    void SpawnSun()
    {
        GameObject sun = SunPool.Instance.GetPooledObject();

        sun.transform.position = new Vector3(Random.Range(limitA.position.x,limitB.position.x),1f, Random.Range(limitA.position.z, limitB.position.z));

        sun.SetActive(true);
    }

}
