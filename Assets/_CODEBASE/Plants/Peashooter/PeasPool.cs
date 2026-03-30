using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasPool : PoolSystem
{
    public static PoolSystem Instance;

    private void Awake()
    {
        Instance = this;
    }
}
