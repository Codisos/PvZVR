using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshPro countText0;
    [SerializeField] private TextMeshPro countText1;
    [SerializeField] private TextMeshPro countText2;
    [SerializeField] private TextMeshPro countText3;

    private List<TextMeshPro> TMlist = new List<TextMeshPro>();
    private float[] plantTimout = { 0, 0, 0, 0 };
    private int _selectedPlant = 0;

    private void Awake()
    {
        TimeoutManager.PlantTimeoutEvent += TimoutEvent;
        GunHand.changeSelectedPlant += ChangeSelectedPlant;

        TMlist.Add(countText0);
        TMlist.Add(countText1);
        TMlist.Add(countText2);
        TMlist.Add(countText3);

    }

    void ChangeSelectedPlant(int index)
    {
        _selectedPlant = index;

        EnableIndex();

    }

    void EnableIndex()
    {
        for(int x = 0; x < 4; x++)
        {
            TMlist[x].gameObject.SetActive(false);
        }

        TMlist[_selectedPlant].gameObject.SetActive(true);
    }

    private void TimoutEvent(int index, bool status, float seconds)
    {

            plantTimout[index] = seconds;

    }




    private void Update()
    {
        for(int i = 0; i < plantTimout.Length ; i++)
        {
            if(plantTimout[i] > 0)
            {
                plantTimout[i] -= Time.deltaTime;
                int z = (int)plantTimout[i];
                TMlist[i].text = z.ToString();
            }
        }

        //change text every frame
    }
}
