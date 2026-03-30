using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int poolAmount;

    private bool poolObjectSent = false;

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < poolAmount; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        poolObjectSent = false;
        
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            
            if (!pooledObjects[i].activeInHierarchy)
            {
                poolObjectSent = true;
                return pooledObjects[i];
            }
        }

        if (!poolObjectSent)        //if pool is not big enough add one more object and return it 
        {
            GameObject temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
            return temp;
        }

        return objectToPool;
    }
}
