using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IDamage
{

    [SerializeField]private int hp;
    [SerializeField] private int cost;
    private bool _isDead=false;

    private Collider plantedOn;   //collider from grid

    public bool isDead => _isDead;

    public event Action OnPlantDeath;

    public void OnPlanted(Collider c)
    {
        SetPlantCordCollider(c);
        GridManager.Instance.SetTile(c.transform.position, this);
    }

    public int GetCost()
    {
        return cost;
    }

    private void SetPlantCordCollider(Collider c)
    {
        plantedOn = c;
        plantedOn.enabled = false;
    }

    private void Awake()
    {
        OnSpawn();
    }

    public void Hit(int damageReceived)
    {
        hp = hp - damageReceived;
        if (hp <= 0 && !_isDead)
        {
            PlantDeath();
        }
    }

    void Replant()
    {
        PlantDeath();
    }

    public virtual void OnSpawn() //Init from wave system
    {
        
    }

    void PlantDeath()
    {
        _isDead = true;  //set because of update

        OnPlantDeath?.Invoke();

        GridManager.Instance.SetTile(transform.position,null);

        gameObject.SetActive(false);

        if(plantedOn != null) plantedOn.enabled = true;
    }

    IEnumerator WaitForDelete()
    {
        yield return new WaitForFixedUpdate();  //wait for plant move
        gameObject.SetActive(false);
        /*gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;*/

        //StartCoroutine(WaitForDestroy());   //delete after plant moved

    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }



}
