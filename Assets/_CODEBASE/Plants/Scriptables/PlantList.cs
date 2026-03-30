using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Plant List", menuName = "My Assets/Plants/PlantList")]
public class PlantList : ScriptableObject
{
    [SerializeField] private List<GameObject> plants;

    public List<GameObject> Plants => plants;
}
