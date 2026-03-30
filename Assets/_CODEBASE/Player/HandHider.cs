using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//KOD CO PATRI NA NADRAZENE EMPTY OBJEKTY RUKOU A PRIJMA CALLS Z HIDE HANDS ON SELECT

public class HandHider : MonoBehaviour
{
    [SerializeField]private GameObject hand;
    public void DisableHand()
    {
        hand.SetActive(false);
    }

    public void EnableHand()
    {
        hand.gameObject.SetActive(true);
    }
}
