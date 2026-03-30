using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyerForwarder : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        gameObject.GetComponentInParent<ParticleDestroyer>().OnSystemStopped();
    }
}
