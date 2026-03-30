using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class GunHandleSwitch : XRBaseInteractable
{

    [Header("Bounds")]
    [SerializeField] private Transform modeA;
    [SerializeField] private Transform modeB;

    [SerializeField] Transform OrigGrabEmpty;

    [Header("Haptics")]
    [SerializeField] XRBaseController controllerRef;
    [Range(0, 1)]
    [SerializeField] float changeModeImpulseForce;
    [Range(0, 10)]
    [SerializeField] float changeModeImpulseDuration;

    //handle tracking stuff
    private Transform inputObject;
    private Vector3 grabStart;
    private bool canMove = false;
    private float startPercent = 0f;


    //handle glide to target mode
    private float distanceToTarget = 0f;
    private float glideTreshold = 0.005f;
    private float duration = 0.03f;

    //change mode stuff
    private bool _isSunMode = false;
    private bool _isPlantMode = true;
    private bool _isLocked = false;     //in case the player is holding trigger, add listener to trigger held
    private bool _inPosOnce = false;

    public static event Action SunModeSelected;
    public static event Action PlantModeSelected;




    // Update is called once per frame
    void Update()
    {
        if (canMove && !_isLocked)
        {
            UpdatePosition();
            SetFireMode();
            CheckIfHandleInPosition();
        }

    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        //vypocitat start perc
        grabStart = transform.position;
        startPercent = Vector3.Distance(grabStart, modeA.position) / Vector3.Distance(modeA.position, modeB.position);
        StartMoving(args);

        base.OnSelectEntered(args);
    }

    //pro pripad ze hrac pusti item zatim co ho zveda pri control
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Release();

        base.OnSelectExited(args);
    }

    void Release()
    {
        canMove = false;
        SetReleasePos();

        if (_isPlantMode)
        {
            //event plant is active
            PlantModeSelected?.Invoke();
            
        }
        else
        {
            //event sun is active
            SunModeSelected?.Invoke();
            
        }

        if(distanceToTarget > glideTreshold)
        {
            StartCoroutine(LerpHandle());
        }

    }

    void CheckIfHandleInPosition()
    {
        if (distanceToTarget < 0.005f && !_inPosOnce)
        {
            OnHandleInPosition();
            _inPosOnce = true;
        }
        else if(distanceToTarget > 0.005f)
        {
            _inPosOnce = false;
        }
    }

    void OnHandleInPosition()
    {
        controllerRef.SendHapticImpulse(changeModeImpulseForce,changeModeImpulseDuration);
        //add sound
    }

    void StartMoving(SelectEnterEventArgs args)
    {
        SetInputObject(args.interactorObject.transform);
        SetOrigGrabPos();
        canMove = true;
        //zacit prehravat zvuk v fmodu vol 0
    }

    void UpdatePosition()
    {
        float newMovePercent = Mathf.Clamp01(startPercent + FindPercentageDifference());
        Vector3 newPos = Vector3.Lerp(modeA.position, modeB.position, newMovePercent);

        Quaternion newRot = transform.rotation;

        transform.SetPositionAndRotation(newPos, newRot);

    }

    private float FindPercentageDifference()
    {
        Vector3 handPos = inputObject.position;
        Vector3 pullDirection = handPos - OrigGrabEmpty.position;
        Vector3 targetDirection = modeB.position - modeA.position;  //OR A - B (DEPENDS ON SELECTED MODE)

        float length = targetDirection.magnitude;
        targetDirection.Normalize();

        return Vector3.Dot(pullDirection, targetDirection) / length;
    }

    void SetFireMode()
    {
        float distanceToPlant = Vector3.Distance(transform.position, modeA.position);
        float distanceToSun = Vector3.Distance(transform.position, modeB.position);

        if (distanceToPlant > distanceToSun)
        {
            _isSunMode = true;
            _isPlantMode = false;
            distanceToTarget = distanceToSun;
            //event sun is selected?
        }
        else
        {
            _isSunMode = false;
            _isPlantMode = true;
            distanceToTarget = distanceToPlant;
            //event plant is selected (neco na zbrani blikne nebo tak...)
        }
    }

    void SetInputObject(Transform input)
    {
        inputObject = input;
    }

    void SetOrigGrabPos()
    {
        //SET CONTRTOLLER POS AS A ORIGINAL GRAB POSITION

        OrigGrabEmpty.position = inputObject.position;
    }

    void SetReleasePos()
    {
        OrigGrabEmpty.position = transform.position;
    }

    IEnumerator LerpHandle()
    {
        float timeE = 0;

        

        while (timeE < duration)
        {
            float t = timeE / duration;
            Vector3 mode = _isPlantMode ? modeA.position : modeB.position;
            transform.SetPositionAndRotation(Vector3.Lerp(OrigGrabEmpty.position, mode, t), transform.rotation);

            timeE += Time.deltaTime;

            yield return null;
        }
        OnHandleInPosition();
    }


}
