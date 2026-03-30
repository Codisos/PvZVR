using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagEmission : MonoBehaviour
{
    [SerializeField] MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        SunsManager.sunAdded += ChangeMag;
        SunsManager.sunSubtracted += ChangeMag;

        ChangeMag(0);
    }

    void ChangeMag(int amount)
    {
        float c = Mathf.Clamp( SunsManager.Instance.GetCurrentSunCount(),0,400);

        rend.materials[1].SetColor("_EmissionColor", Color.white * c / 400);

    }
}
