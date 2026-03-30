using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDetection : MonoBehaviour
{
    [SerializeField] ZombieDamage zombieDamObj;
    [SerializeField] ZombieMovement movement;

    private Plant lastPlant;
    private GameObject lastPlantObject;
    private bool _isDetected = false;

    //control raycast
    private int layerMask = 1 << 6;
    private RaycastHit hit;

    private void OnEnable()
    {
        //GridManager.Instance.GridUpdated += CheckAfterGridUpdate;
        movement.OnRowChange += CheckWhenTileCrossed;
    }
    private void OnDisable()
    {
        //GridManager.Instance.GridUpdated -= CheckAfterGridUpdate;
    }

    /*private void OnTriggerEnter(Collider collider)
    {

        collider.gameObject.TryGetComponent<Plant>(out lastPlant);

        if (lastPlant != null)
        {
            lastPlantObject = collider.gameObject;
            
            _isDetected = true;
            StartCoroutine(AttackCountdown(lastPlantObject));
            movement.PauseMovement();
        }
    }*/

    private void OnPlantDetected(GridTile tile)
    {
        lastPlant = tile.placedPlant;

        if (lastPlant != null && !_isDetected)
        {
            lastPlant.OnPlantDeath += AttackedPlantDied;

            lastPlantObject = lastPlant.gameObject;

            _isDetected = true;
            StartCoroutine(AttackCountdown(lastPlantObject));
            movement.PauseMovement();
        }
        //else if() //plant null and is attacking? -> stop attack and go
    }

    /*private void OnTriggerExit(Collider other)
    {
        EndDetection();
    }*/

    IEnumerator AttackCountdown(GameObject c)
    {
        
            while (_isDetected)
            {
                zombieDamObj.Attack(c);
                yield return new WaitForSeconds(2);    //+last zombie spawn

                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, layerMask))
                {

                }
                else
                {
                    EndDetection();
                }
            }

            if (lastPlantObject != null && !c.activeInHierarchy)
            {
                EndDetection();
            }
        
    }

    void EndDetection()
    {
        if(lastPlant != null) lastPlant.OnPlantDeath -= AttackedPlantDied;

        _isDetected = false;
        zombieDamObj.StopAttacking();
        movement.ResumeMovement();
        lastPlant = null;
        lastPlantObject = null;
    }

    void CheckAfterGridUpdate(GridTile tile)
    {
        GridCord zombieCords = GridManager.Instance.GetTileCord(transform.position);

        if (tile.cords.Equals(zombieCords) || tile.cords.Equals(zombieCords.row + 1))
        {
            OnPlantDetected(tile);
        }
        if (tile.placedPlant == null)//what if +1 has a tile there? check pos periodical
            EndDetection();
    }

    void CheckWhenTileCrossed(GridCord cords)
    {
        GridTile newTile = GridManager.Instance.CheckForPlantOnTile(cords);

        if(newTile != null)
        {
            OnPlantDetected(newTile);
        }
    }

    void AttackedPlantDied()
    {
        EndDetection();
    }

}
