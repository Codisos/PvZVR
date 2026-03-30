using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorZombiePool : PoolSystem
{
    public static ArmorZombiePool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
