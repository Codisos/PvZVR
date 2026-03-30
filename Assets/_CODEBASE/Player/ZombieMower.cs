using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMower : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform _endPos;
    [SerializeField] float stopTime = 5f;
    [SerializeField] GameObject particles;

    private bool canMove = false;
    private Rigidbody rb;
    private Vector3 curPos;
    private float endBuffer = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            curPos = transform.position;

            float distance = Vector3.Distance(curPos, _endPos.position);

            if (distance > endBuffer)
            {
                Vector3 dir = _endPos.position - curPos;
                dir.Normalize();

                rb.MovePosition(curPos + (dir * speed * Time.deltaTime));
            }
            else
            {
                EndMovement();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canMove)
        {
            StartMoving();
        }
        else
        {
            DamageZombieOnCollision(other);
        }
    }

    void StartMoving()
    {
        canMove = true;
        StartCoroutine(EndMovementCountDown());

        //ani
        Animation ani = GetComponent<Animation>();
        ani.Play();

        //sound

        //particles
        particles.SetActive(true);
    }

    void EndMovement()
    {
        canMove = false;
        gameObject.SetActive(false);
    }

    IEnumerator EndMovementCountDown()
    {
        yield return new WaitForSeconds(stopTime);
        EndMovement();
    }

    void DamageZombieOnCollision(Collider col)
    {
        IDamage hit;

        col.gameObject.TryGetComponent<IDamage>(out hit);

        hit?.Hit(100);
    }

}
