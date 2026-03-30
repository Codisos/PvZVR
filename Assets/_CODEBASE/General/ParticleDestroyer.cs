using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private int callbackCount = 0;
    private int childCount = 0;
    
    //particle destroy forwarder jde automatizovat, staci dat do awake "dej children do array a pak ke kaydzmu pridej komponent PDForwarder"

    public void OnSystemStopped()
    {
        callbackCount++;

        childCount = GetComponent<ParticleSystem>() ? childCount = (transform.childCount + 1) : childCount = transform.childCount;

        if (callbackCount == transform.childCount)
        {
            Destroy(gameObject);
        }
    }

}
