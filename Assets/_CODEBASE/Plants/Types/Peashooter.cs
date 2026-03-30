using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{

    [SerializeField] Transform projectileSpawnPos;

    private int layerMask = 1 << 7;

    private Animator animator;


    public override void OnSpawn()
    {
        base.OnSpawn();

        //make listener
        PlantShootManager.PeashooterShootEvent += RequestShoot;
        //get reference to animator
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        PlantShootManager.PeashooterShootEvent -= RequestShoot;
    }

    void RequestShoot()
    {
        bool checkZ = CheckForZombieInLine();

        if (checkZ)
        {
            SpawnProjectile();
        }
    }


    bool CheckForZombieInLine()
    {
        var ray = new Ray(transform.position, transform.right * -1);
        RaycastHit hit;
        if (Physics.Raycast(projectileSpawnPos.position, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void SpawnProjectile()
    {
        animator.SetTrigger("Attack");

        GameObject newProjectile = PeasPool.Instance.GetPooledObject();

        newProjectile.transform.position = projectileSpawnPos.position;

        newProjectile.SetActive(true);

        GetComponent<FMODOneShotEventStack>().PlayOneShot(GetComponent<FMODOneShotEventStack>().sounds[0]);

    }

}
