using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDisplayUI : MonoBehaviour
{
    [SerializeField] MeshRenderer displayObjectRend;


    private Vector2[] UVPlantPositions = {new Vector2(0f,0f), new Vector2(0.5f, 0f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f)};
    private int _selectedPlant = 0;

    //DON'T TOUCH! tohle je primo uloziste pro aktualni cas do konce odpoctu u kazde kytky
    private float[] plantTimout = { 0, 0, 0, 0 };

    //DON'T TOUCH! konverze pro nacitani na displeji
    private float[] timeoutForDisplay = { 1, 1, 1, 1 };

    ////DON'T TOUCH! obchazeni kvuli konverzi toto pole se meni pokud je na indexu 0 (prichozi action se vola bud s max value nebo 0, proto to funguje)
    private float[] timeoutMaxValue = { 0, 0, 0, 0 };


    private void Awake()
    {
        TimeoutManager.PlantTimeoutEvent += TimoutEvent;
        GunHand.changeSelectedPlant += OnChangeSelectedPlant;
    }

    void OnChangeSelectedPlant(int index)
    {
        //update current plant index
        _selectedPlant = index;

        //set display image
        displayObjectRend.material.SetFloat("_UVOffsetX", UVPlantPositions[index].x);
        displayObjectRend.material.SetFloat("_UVOffsetY", UVPlantPositions[index].y);
    }

    //prichozi event z timeout manageru, displej potrebuje jen vedet sekundy (chodi totiz jen info na zacatku a na konci timeoutu - 0/1)
    private void TimoutEvent(int index, bool status, float seconds)
    {

        plantTimout[index] = seconds;

        if(timeoutMaxValue[index] == 0)
        {
            timeoutMaxValue[index] = seconds;
        }

    }

    //zajistuje update vsech potrebnych poli ale uz se nestara o zobrazeni
    // je spusteny z updatu
    private void UpdateArrays()
    { 
        for (int i = 0; i < plantTimout.Length; i++)
        {
            if (plantTimout[i] > 0)
            {
                plantTimout[i] -= Time.deltaTime;
                timeoutForDisplay[i] = (timeoutMaxValue[i] - plantTimout[i]) / timeoutMaxValue[i];
            }
        }
    }

    //zajistuje jen a pouze zobrazeni timeoutu prave selectnute kytky
    private void UpdateDisplayTimeout()
    {
        displayObjectRend.material.SetFloat("_Progress", Mathf.Clamp01(timeoutForDisplay[_selectedPlant]));
    }

    private void Update()
    {
        UpdateArrays();

        UpdateDisplayTimeout();
    }
}
