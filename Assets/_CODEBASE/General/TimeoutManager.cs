using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutManager : MonoBehaviour
{
    [SerializeField] FloatReference peashooterWait;
    [SerializeField] FloatReference sunflowerWait;
    [SerializeField] FloatReference cherryWait;
    [SerializeField] FloatReference wallnutWait;

    public static event Action<int, bool,float> PlantTimeoutEvent;  //float is for seconds remaining to end of countdown(will use in UI for its own countdown element)
    private int i = 0;

    private void Awake()
    {
        PlantSpawnSystem.FlowerPlanted += TimeoutRequest;
        GunHand.requestToShootTimeout += TimeoutRequest;
    }

    void TimeoutRequest(int index)
    {
        i = index;

        switch (index)
        {
            case 0:
                Peashooter();
                break;
            case 1:
                Sunflower();
                break;
            case 2:
                Cherry();
                break;
            case 3:
                Wallnut();
                break;

            default:
                Debug.LogWarning("Wrong index sent to TimoutManager");
                break;
        }

    }

    void Peashooter()
    {
        PlantTimeoutEvent?.Invoke(0, false,peashooterWait.Value);
        StartCoroutine(PeashooterCountdown());
    }

    void Sunflower()
    {
        PlantTimeoutEvent?.Invoke(1, false,sunflowerWait.Value);
        StartCoroutine(SunflowerCountdown());
    }

    void Wallnut()
    {
        PlantTimeoutEvent?.Invoke(3, false,wallnutWait.Value);
        StartCoroutine(WallnutCountdown());
    }

    void Cherry()
    {
        PlantTimeoutEvent?.Invoke(2, false,cherryWait.Value);
        StartCoroutine(CherryCountdown());
    }

    //-------------------------------------------------------------COUNTDOWN------------------------------------------------------

    IEnumerator PeashooterCountdown()
    {
        yield return new WaitForSeconds(peashooterWait.Value);
        PlantTimeoutEvent?.Invoke(0, true,0);
    }
    IEnumerator SunflowerCountdown()
    {
        yield return new WaitForSeconds(sunflowerWait.Value);
        PlantTimeoutEvent?.Invoke(1, true,0);
    }
    IEnumerator CherryCountdown()
    {
        yield return new WaitForSeconds(cherryWait.Value);
        PlantTimeoutEvent?.Invoke(2, true,0);
    }
    IEnumerator WallnutCountdown()
    {
        yield return new WaitForSeconds(wallnutWait.Value);
        PlantTimeoutEvent?.Invoke(3, true,0);
    }
}
