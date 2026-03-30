using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//KOD CO JE PRIMO NA HANDS MODELU A CALLUJE HAND HIDER CO JE NA NADRAZENEM EMPTY OBJEKTU
public class HideHandsOnSelect : MonoBehaviour
{
    [SerializeField] private MeshRenderer handRenderer;
    [SerializeField] private XRDirectInteractor directGrab;

    private void Start()    //ready for later optimalization, mozna bude vic veci co se subnou k  interactorum na startu
    {
        directGrab.selectEntered.AddListener(HideHand);
        directGrab.selectExited.AddListener(UnhideHand);

    }


    public void HideHand(BaseInteractionEventArgs eventArgs)
    {
        handRenderer.enabled = false;
    }

    public void UnhideHand(BaseInteractionEventArgs eventArgs)
    {
        handRenderer.enabled = true;
    }

    private void OnDestroy()
    {
        directGrab.selectEntered.RemoveListener(HideHand);
        directGrab.selectExited.RemoveListener(HideHand);
    }
}
