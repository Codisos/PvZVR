using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeZombiePool : PoolSystem
{
    public static ConeZombiePool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
