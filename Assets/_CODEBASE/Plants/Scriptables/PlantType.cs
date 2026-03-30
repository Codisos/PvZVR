using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Plant", menuName = "My Assets/Plants/PlantType")]
public class PlantType : ScriptableObject
{
    [SerializeField] private int hp;
    [SerializeField] private int damage;


    public int HP => hp;
    public int Damage => damage;

}
