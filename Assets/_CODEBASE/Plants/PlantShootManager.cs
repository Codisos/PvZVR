using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShootManager : MonoBehaviour
{
    [SerializeField] private FloatReference peashooterFireRate;

    public static event Action PeashooterShootEvent;

    private void Start()
    {
        StartCoroutine(PeashooterCountdown());
    }

    IEnumerator PeashooterCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(peashooterFireRate.Value);
            PeashooterShootEvent?.Invoke();
        }
    }

}
