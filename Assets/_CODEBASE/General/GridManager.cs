using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Transform validminmin;
    public Transform validmaxmax;
    public float tileScale = 1.5f;

    private float validXRange;
    private float validYRange;

    private List<GridTile> grid = new List<GridTile>();

    Dictionary<float, string> columnHash = new Dictionary<float, string>();

    public event Action<GridTile> GridUpdated;

    private void Awake()
    {
        Instance = this;

        columnHash.Add(3,"E");
        columnHash.Add(1.5f, "D");
        columnHash.Add(0, "C");
        columnHash.Add(-1.5f, "B");
        columnHash.Add(-3, "A");
    }

    public void SetTile(Vector3 pos, Plant plant)
    {
        GridTile tile = new GridTile(GetTileCord(pos),plant);
        
        if (tile != null)
        {
            //check for the same tile
            for (int i = 0; i < grid.Count; i++)
            {
                if (grid[i].Equals(tile))
                {
                    if (tile.placedPlant != null)
                        grid[i] = tile;
                    else
                        grid.RemoveAt(i);

                    return;
                }
            }

            if(tile.placedPlant != null)
                grid.Add(tile);

            GridUpdated?.Invoke(tile);
        }
        
    }

    public GridTile CheckForPlantOnTile(GridCord cord)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i].cords.Equals(cord))
            {

                return grid[i];
            }
        }

        return null;
    }

    public GridCord GetTileCord(Vector3 pos)
    {

        int[] cords = CalcValidCord(pos.x, pos.z);
        int x = cords[0] *-1;

        if ( pos.x > validminmin.position.x + tileScale / 2)
        {
            x = -1;
        }
        else if(pos.x < (validmaxmax.position.x - tileScale / 2))
        {
            x = 8;
        }

        if (x == 0) x = 1;

        return new GridCord(columnHash[pos.z], x);

    }


    int[] CalcValidCord(float x, float z)
    {
        float minX = validminmin.position.x + tileScale / 2;
        float maxX = validmaxmax.position.x - tileScale / 2;

        float minZ = validminmin.position.z - tileScale / 2;
        float maxZ = validmaxmax.position.z + tileScale / 2;

        int[] cords = { (int)Math.Round(((x - minX) / 1.5f)), Mathf.RoundToInt(minZ - z) };

        return cords;
    }
}
