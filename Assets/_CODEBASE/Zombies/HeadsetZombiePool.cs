using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetZombiePool : PoolSystem
{
    public static HeadsetZombiePool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
