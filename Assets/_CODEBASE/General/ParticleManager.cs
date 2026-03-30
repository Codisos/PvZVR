using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject seedImpact;
    [SerializeField] GameObject muzzleFlash;

    private void Awake()
    {
        GunHand.RequestImpactParticles += SpawnImpactParticles;
        GunHand.RequestShootParticles += SpawnShootParticles;
    }

    void SpawnImpactParticles(Quaternion rot, Vector3 pos)
    {
        Instantiate<GameObject>(seedImpact,pos,rot);
    }

    void SpawnShootParticles(Transform posRot)
    {
        Instantiate<GameObject>(muzzleFlash, posRot.position, posRot.rotation);
    }
}
