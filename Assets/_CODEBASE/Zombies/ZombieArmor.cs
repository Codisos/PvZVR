using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieArmor : MonoBehaviour
{
    [SerializeField] IntReference hp;

    [SerializeField] GameObject[] armorObjects;

    [SerializeField] int allArmorSum = 0;

    [SerializeField] GameObject particle;

    private int _hp;
    private void Start()
    {
        _hp = hp.Value;

        _hp += allArmorSum;

    }

    public void ArmorDamage(int damageRec)
    {
        _hp -= damageRec;
        CheckDamage();
    }

   void CheckDamage()
   {
        if(_hp == 0)
        {
            
            //disable this armor obj
            armorObjects[0].SetActive(false);

            if (armorObjects.Length == 2)
            {
                armorObjects[1].SetActive(false);
            }

            //play ani/particle
            Instantiate(particle, transform);

            GetComponentsInParent<FMODOneShotEventStack>()[0].PlayOneShot(GetComponentInParent<FMODOneShotEventStack>().sounds[2]);

            return;
        }
        
        if (_hp <= hp.Value / 2 && armorObjects.Length == 2)
        {
            armorObjects[0].SetActive(false);
            armorObjects[1].SetActive(true);
        }
   }
}
