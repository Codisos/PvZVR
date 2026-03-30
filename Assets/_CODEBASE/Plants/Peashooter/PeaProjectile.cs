using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaProjectile : Damage
{
    [SerializeField] IntReference damage;
    [SerializeField] FloatReference moveSpeed;
    [SerializeField]private Rigidbody rb;
    [SerializeField] private GameObject impactVFX;

    private GameObject go;



    private void OnEnable()
    {

        //reset velocity
        rb.velocity = Vector3.zero;

        //add velocity
        rb.AddForce(this.transform.right * -1 * moveSpeed.Value);



    }


    private void OnTriggerEnter(Collider other)
    {
        go = other.gameObject;

        if (go.GetComponent<IDamage>() != null)
        {
            DealDamage(go, damage.Value);
            
        }



        Instantiate(impactVFX, gameObject.transform.position, gameObject.transform.rotation);

        gameObject.SetActive(false);
    }
}
