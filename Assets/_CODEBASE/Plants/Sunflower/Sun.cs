using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private IntReference sunAddAmmount;
    [SerializeField] float SpawnEjectPower = 70;
    [SerializeField] Vector3Reference _targetPos;

    [SerializeField]private Rigidbody rb;


    [Header("DEBUG")]
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    private bool moveToGun = false;

    private void Start()
    {
        GunHand.activateSunSuck += SunSuckRequest;
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);

        rb.velocity = Vector3.zero;

        //add force to sun in random direction so it "falls out"
        rb.AddForce(new Vector3(Random.Range(-1f,1f),transform.position.y * 1.5f, Random.Range(-1f, 1f)) * SpawnEjectPower);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnCancelSuck()
    {
        rb.isKinematic = false;
        transform.localScale = new Vector3(1, 1, 1);
        rb.AddForce(Vector3.Normalize(transform.position - _targetPos.Value));
    }

    private void SunSuckRequest(bool status)
    {
        if (status)
        {
            moveToGun = true;
        }
        else
        {
            moveToGun = false;
            OnCancelSuck();
        }
    }

    private void FixedUpdate()
    {
        if (moveToGun)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos.Value, _speed * Time.deltaTime);

            float x = Vector3.Distance(transform.position, _targetPos.Value);
            transform.localScale = transform.localScale * Mathf.Clamp01(x/2);

            if ( x < _distance)
            {
                //add points
                SunsManager.Instance.CountUp(sunAddAmmount.Value);
                moveToGun = false;
                gameObject.SetActive(false);
            }
        }
    }

}
