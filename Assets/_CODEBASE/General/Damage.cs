using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public void DealDamage(GameObject target, int damage)
    {
        target.GetComponent<IDamage>().Hit(damage);
    }
}
