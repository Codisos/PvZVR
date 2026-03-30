using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private PlantList plantList;
    [SerializeField] private TextMeshPro textObject;

    private void Awake()
    {
        GunHand.changeSelectedPlant += SetDisplayName;
    }

    void SetDisplayName(int index)
    {
        //set stuff from index as text
        textObject.text = plantList.Plants[index].name;
    }
}
