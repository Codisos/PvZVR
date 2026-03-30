using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CherryBomb : Plant
{
    [SerializeField] GameObject modelToHideOnExplosion;
    [Header("EXPLOSION")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] FloatReference explosionTime;
    [SerializeField] float explosionRadius = 1;
    [SerializeField] LayerMask layer;


    public EventReference ExplosionSound;

    public override void OnSpawn()
    {
        base.OnSpawn();

        //corutine for explosion countdown
        StartCoroutine(ExplosionCountdown());


    }


    void Explosion()
    {
        modelToHideOnExplosion.SetActive(false);

        Instantiate(explosionVFX,gameObject.transform);

        //fmod
        RuntimeManager.PlayOneShot(ExplosionSound ,transform.position);

        RaycastHit[] hit = Physics.SphereCastAll(transform.position,explosionRadius,transform.up, Mathf.Infinity, layer);

        if(hit != null)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                hit[i].transform.gameObject?.GetComponent<IDamage>().Hit(500);
            }
        }
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionTime.Value);
        Explosion();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
