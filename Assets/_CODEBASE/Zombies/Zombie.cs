using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : MonoBehaviour,IDamage
{
    [SerializeField] private ZombieType type;
    [SerializeField] private Renderer[] renderersForHitFlash; 

    private ZombieArmor[] armor; 

    private int hp;

    private Animator animator;

    private FMODOneShotEventStack fmodStack;

    public int HP => hp;

    public static Action ZombieKilled;

    public void OnSpawn(Vector3 endPos) //Init from wave system
    {
        GetComponent<ZombieMovement>().OnSpawned(endPos);

        hp = type.HP;

        animator = GetComponentInChildren<Animator>();

        armor = GetComponentsInChildren<ZombieArmor>();

        animator.SetTrigger("Alive");

        fmodStack = GetComponent<FMODOneShotEventStack>();

        PlayIdleSound();
    }

    public void Hit(int damageReceived)
    {
        hp = hp - damageReceived;

        if (armor != null)
        {
            foreach (ZombieArmor armorpiece in armor)
            {
                if (armorpiece.enabled)
                {
                    armorpiece.ArmorDamage(damageReceived);
                }
            }

        }

        if (hp <= 0)
        {
            //wait for ani

            ZombieDeath();
            MakeHitFlash();
        }

        else
        {
            MakeHitFlash();
        }
    }


    void ZombieDeath()
    {
        GetComponent<ZombieMovement>().PauseMovement();
        //GetComponent<ZombieMovement>().SetNewEndPoint(transform.position);

        ZombieKilled?.Invoke();

        animator.SetTrigger("Death");

        //particles?

        StartCoroutine(DespawnCountDown());
    }

    void MakeHitFlash()
    {
        StartCoroutine(FlashCorutine());
    }

    IEnumerator FlashCorutine()
    {
        //flash
        foreach (Renderer rend in renderersForHitFlash)
        {
            if (rend.gameObject.activeInHierarchy)
            {
                rend.material.EnableKeyword("_EMISSION");
                rend.material.SetColor("_EmissionColor", Color.white);
            }
        }

        //wait
        yield return new WaitForSeconds(0.1f);

        //disable flash
        foreach (Renderer rend in renderersForHitFlash)
        {
            rend.material.SetColor("_EmissionColor", Color.black);
            rend.material.DisableKeyword("_EMISSION");
        }
    }

    IEnumerator DespawnCountDown()
    {
        //wait
        yield return new WaitForSeconds(2.6f);

        //disable gameobject
        gameObject.SetActive(false);
    }

    void PlayIdleSound()
    {
        fmodStack.PlayOneShot(fmodStack.sounds[0]);
    }

    

}
