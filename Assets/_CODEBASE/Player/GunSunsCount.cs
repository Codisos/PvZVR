using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSunsCount : MonoBehaviour
{
    [SerializeField] private TextMeshPro textObject;


    void Start()
    {
        SunsManager.sunAdded += SetDisplayPlus;
        SunsManager.sunSubtracted += SetDisplayMinus;

        textObject.text = SunsManager.Instance.GetCurrentSunCount().ToString();
    }

    void SetDisplayPlus(int amount)
    {

        textObject.text = SunsManager.Instance.GetCurrentSunCount().ToString();
    }

    void SetDisplayMinus(int amount)
    {

        textObject.text = SunsManager.Instance.GetCurrentSunCount().ToString();
    }

}
