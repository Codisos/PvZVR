using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamage : Damage
{
    [SerializeField] private ZombieType zombieType;
    int _damage;

    private bool _isAttacking = false;
    private Animator animator;
    private FMODOneShotEventStack fmodStack;

    private void Start()
    {
        _damage = zombieType.Damage;
        animator = GetComponentInChildren<Animator>();
        fmodStack = GetComponent<FMODOneShotEventStack>();
    }

    public void Attack(GameObject target)
    {
        _isAttacking = true;

        animator.SetTrigger("Attack");

        DealDamage(target, _damage);

        PlayAttackSound();
    }

    public void StopAttacking()
    {
        _isAttacking = false;
    }

    void PlayAttackSound()
    {
        fmodStack.PlayOneShot(fmodStack.sounds[1]);
    }

}
