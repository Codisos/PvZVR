using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : PoolSystem
{
    public static ZombiePool Instance;

    private void Awake()
    {
        Instance = this;
    }

}
