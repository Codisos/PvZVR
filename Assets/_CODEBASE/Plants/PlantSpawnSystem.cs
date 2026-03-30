using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawnSystem : MonoBehaviour
{
    [SerializeField] private PlantList listOfPlants;
    [SerializeField] private List<GameObject> AllGridPoints;  //not needed?

    public static PlantSpawnSystem Instance;

    public static event Action<int> FlowerPlanted;


    private void Awake()
    {
        Instance = this;

        GunHand.requestToPlant += RequestPlant;
    }

    void RequestPlant(Collider cordsCollider, int plantPrefabIndex)
    {
        PlantFlower(cordsCollider,plantPrefabIndex);
    }

    void RequestReplant()
    {

    }

    void PlantFlower(Collider c,int index)
    {
        GameObject planted = Instantiate(listOfPlants.Plants[index],c.transform);
        planted.GetComponent<Plant>().OnPlanted(c);
        FlowerPlanted?.Invoke(index);
    }
}
