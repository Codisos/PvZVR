using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GunHand : MonoBehaviour
{
    [SerializeField] private InputActionReference trigger;
    [SerializeField] private InputActionReference switchFwd;
    [SerializeField] private InputActionReference switchBwd;
    [SerializeField] XRBaseController controllerREf;
    [SerializeField] private PlantList listOfPlants;
    [SerializeField] private IntReference _gunDamage;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Vector3Reference gunPos;

    public static event Action<Collider, int> requestToPlant;
    public static event Action<int> requestToShootTimeout;
    public static event Action<int> changeSelectedPlant;
    public static event Action<bool> triggerHeld;
    public static event Action<bool> activateSunSuck;
    public static event Action<Quaternion, Vector3> RequestImpactParticles;
    public static event Action<Transform> RequestShootParticles;

    private bool canShoot = true;
    private int _currentPlantIndex = 0;

    private bool _isPlantMode = true;
    private bool _triggerIsHeld = false;

    private bool[] plantsOnTimeout = { true, true, true, true };

    //make layer mask
    private int mask1 = 1 << 6;
    private int mask2 = 1 << 7;
    private int mask3 = 1 << 10;
    private int mask4 = 1 << 0;

    //fmod
    private FMODOneShotEventStack oneShotSoundStack;
    private FMODUnity.EventReference[] soundsInOneShotStack;


    private void Start()
    {
        //shoot
        trigger.action.performed += TriggerPressed;
        trigger.action.canceled += TriggerExit;
        trigger.action.Enable();

        //switch plant
        switchFwd.action.performed += SwitchPlantFwd;
        switchFwd.action.Enable();
        switchBwd.action.performed += SwitchPlantBwd;
        switchBwd.action.Enable();

        //can shoot (timeout)
        TimeoutManager.PlantTimeoutEvent += SetCanShoot;

        //gun handle changing modes
        GunHandleSwitch.PlantModeSelected += ChangeToPlantMode;
        GunHandleSwitch.SunModeSelected += ChangeToSunMode;

        //get fmod stack
        oneShotSoundStack = GetComponent<FMODOneShotEventStack>();
        soundsInOneShotStack = oneShotSoundStack.sounds;
    }

    void ChangeToPlantMode()
    {
        _isPlantMode = true;
        PlayChangeModeSound(0);
    }

    void ChangeToSunMode()
    {
        _isPlantMode = false;
        PlayChangeModeSound(1);
    }

    private int GetCurrentPlantIndex()   //maybe do action with this as an output
    {
        return _currentPlantIndex;
    }

    private void SetCanShoot(int index , bool status, float timeLeftOnIndex) //time left on index is updated only twice, its either 0 or start wait time of plant
    {
        plantsOnTimeout[index] = status;
    }

    private void TriggerExit(InputAction.CallbackContext obj)
    {
        _triggerIsHeld = false;
        triggerHeld?.Invoke(_triggerIsHeld);

        if (!_isPlantMode)
        {
            activateSunSuck?.Invoke(false);
        }
    }
        private void TriggerPressed(InputAction.CallbackContext obj)
    {
        _triggerIsHeld = true;
        triggerHeld?.Invoke(_triggerIsHeld);

        //check mode if/if?

        if (_isPlantMode)
        {
            int plantCost = listOfPlants.Plants[_currentPlantIndex].GetComponent<Plant>().GetCost();

            //check if selected plant is available + check if player has suns
            canShoot = plantsOnTimeout[_currentPlantIndex] && SunsManager.Instance.GetCurrentSunCount() >= plantCost? true : false;


            if (canShoot)
            {
                PlayShootingSound();

                //combine layer mask
                var layerMask = mask2 | mask3 | mask4;

                var ray = new Ray(rayOrigin.position, transform.forward);
                RaycastHit hit;

                //trigger raycast
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
                {
                    RequestShootParticles?.Invoke(rayOrigin.transform);

                    if (hit.collider.GetComponent<Grid>() != null)
                    {
                        HitGrid(hit.collider, hit.transform);
                    }
                    else if (hit.collider.GetComponent<Zombie>() != null)
                    {
                        requestToShootTimeout?.Invoke(_currentPlantIndex);
                        HitZombie(hit);
                    }
                    else if (hit.collider.GetComponent<Plant>() != null)
                    {
                        HitPlant(hit.transform);
                    }   //will not work, replanting is not possible with a gun like this
                    else
                    {
                        //default layer
                        ImpactPoint(hit.transform.rotation, hit.point);
                    }
                    SunsManager.Instance.CountDown(plantCost);
                }
            }
        }
        else
        {
            activateSunSuck?.Invoke(true);

            //play suck sound

        }
    }

    void HitZombie(RaycastHit hit)
    {
        hit.collider.GetComponent<IDamage>().Hit(_gunDamage.Value);
        ImpactPoint(hit.transform.rotation,hit.point);
    }

    void HitPlant(Transform pos)
    {
        //nejak kytku deletnout
    } 

    void HitGrid(Collider x, Transform pos)
    {
        requestToPlant.Invoke( x, GetCurrentPlantIndex());
        ImpactPoint(pos.rotation, pos.position);
    }

    void ImpactPoint(Quaternion rot, Vector3 pos)
    {
        //do stuff for impact particles
        //event here
        RequestImpactParticles?.Invoke(rot, pos);
    }

    private void SwitchPlantFwd(InputAction.CallbackContext obj)
    {
        _currentPlantIndex++;

        if (_currentPlantIndex >= listOfPlants.Plants.Count)
        {
            _currentPlantIndex = 0;
        }

        changeSelectedPlant?.Invoke(_currentPlantIndex);

        PlayPlantChangeSound();
    }

    private void SwitchPlantBwd(InputAction.CallbackContext obj)
    {
        _currentPlantIndex--;

        if (_currentPlantIndex < 0)
        {
            _currentPlantIndex = listOfPlants.Plants.Count - 1;
        }

        changeSelectedPlant?.Invoke(_currentPlantIndex);

        PlayPlantChangeSound();

    }

    private void FixedUpdate()
    {
        gunPos.Variable.Value = rayOrigin.position;
    }

    //-----------------------FMOD---------------------------------
    void PlayChangeModeSound(float modeParameter)
    {
        oneShotSoundStack.PlayOneShotChangeMode(soundsInOneShotStack[0], modeParameter);
    }

    void PlayShootingSound()
    {
        oneShotSoundStack.PlayOneShot(soundsInOneShotStack[1]);
    }

    void PlayPlantChangeSound()
    {
        oneShotSoundStack.PlayOneShot(soundsInOneShotStack[2]);
    }
}
