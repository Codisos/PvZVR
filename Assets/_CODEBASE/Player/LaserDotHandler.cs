using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDotHandler : MonoBehaviour
{
    [SerializeField] Transform dotObject;
    [SerializeField] Transform rayOrigin;

    private RaycastHit hit;

    int layerMaskA = 1 << 10;
    int layerMaskB = 1 << 8;
    int layerMaskC = 1 << 6;

    int layerMask;

    private void Start()
    {
        layerMask = layerMaskA | layerMaskB | layerMaskC;
    }


    void Update()
    {
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ~layerMask))
        {

            dotObject.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal * -1, hit.transform.up));
        }
        
    }
}
