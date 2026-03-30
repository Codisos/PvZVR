using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPool : PoolSystem
{
    public static SunPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
