using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsManager : MonoBehaviour
{
    public static SunsManager Instance;

    public static event Action<int> sunAdded;       //events for sun count display
    public static event Action<int> sunSubtracted;  

    [SerializeField]private int currentSunCount = 0;

    private void Awake()
    {
        Instance = this;

    }

    public void CountUp(int amount)
    {
        
        currentSunCount += amount;
        sunAdded?.Invoke(amount);
    }

    public void CountDown(int amount)
    {
        currentSunCount -= amount;
        sunSubtracted?.Invoke(amount);
    }

    public int GetCurrentSunCount()     //for display + gun to ask when player wants to plant
    {
        return currentSunCount;
    }
}
