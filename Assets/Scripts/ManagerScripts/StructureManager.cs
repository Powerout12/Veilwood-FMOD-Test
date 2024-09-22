using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureManager : MonoBehaviour
{
    [Header("Tiles")]
    public Tilemap tileMap;
    public TileBase freeTile, occupiedTile;


    void Start()
    {
        
    }


    public Vector3 CheckTile(Vector3 pos)
    {
        Vector3Int gridPos = tileMap.WorldToCell(pos);

        TileBase currentTile = tileMap.GetTile(gridPos);

        if(currentTile != null && currentTile == freeTile)
        {
            //
            Vector3 spawnPos = tileMap.GetCellCenterWorld(gridPos);
            return spawnPos;
        } 
        else return new Vector3 (0,0,0);
    }

    public void SpawnStructure(GameObject obj, Vector3 pos)
    {
        Instantiate(obj, pos, Quaternion.identity);
        Vector3Int gridPos = tileMap.WorldToCell(pos);
        tileMap.SetTile(gridPos, occupiedTile);
    }
}
