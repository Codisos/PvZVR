using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventRaiser : MonoBehaviour
{
    public GameEvent Event;

    public void RaiseEvent()
    {
        Event.Raise();
    }
}
