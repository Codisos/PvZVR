using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastZombieDetection : MonoBehaviour
{
    [SerializeField] private UnityEvent _detected;
    [SerializeField] private UnityEvent _notDetected;

    private int layerMask = 1 << 7;


    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(transform.position,transform.right * -1);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {

            _detected.Invoke();
            
            
        }
        else
        {
            //event
            _notDetected.Invoke();
        }
    }
}
