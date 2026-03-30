using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{
    [SerializeField] private FloatReference sunSpawnRate;
    [SerializeField] private Transform sunSpawnPos;
    

    private void OnEnable()
    {
        StartCoroutine(SpawnSunCountdown());
    }

    IEnumerator SpawnSunCountdown()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(sunSpawnRate.Value);
            SpawnSun();
        }
    }

    void SpawnSun()
    {
        if (gameObject.activeInHierarchy)
        {
            GameObject sun = SunPool.Instance.GetPooledObject();

            sun.transform.position = sunSpawnPos.position;

            sun.SetActive(true);

        }
    }
}
