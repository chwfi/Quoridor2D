using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInfo : MonoBehaviour
{
    public static TilemapInfo Instance;

    private Tilemap _colliderTileMap;
    private Tilemap _groundTileMap;
    private void Awake()
    {
        Instance = this; //싱글톤

        Transform colliderTrm = transform.Find("Collider");
        _colliderTileMap = colliderTrm.GetComponent<Tilemap>();
        Transform groundTrm = transform.Find("Ground");
        _groundTileMap = groundTrm.GetComponent<Tilemap>();

        Debug.Log(_groundTileMap.cellBounds);
    }
    
    
    void Start()
    {
        TileBase tb = _groundTileMap.GetTile(new Vector3Int(0, 0, 0));
        
        Debug.Log($"x : {_groundTileMap.cellBounds.xMin}, {_groundTileMap.cellBounds.xMax}");
        Debug.Log($"y : {_groundTileMap.cellBounds.yMin}, {_groundTileMap.cellBounds.yMax}");


        _groundTileMap.SetTile(new Vector3Int(0, 0, 0), null);
        
    }

    public Vector3Int GetStartCellPos()
    {
        return new Vector3Int(-1, 4, 0);
    }

    public Vector3 GetWorldPos(Vector3Int pos)
    {
        return _groundTileMap.GetCellCenterWorld(pos);
    }

    public Vector3Int GetCellPos(Vector3 pos)
    {
        return _groundTileMap.WorldToCell(pos);
    }

    public bool CanMove(Vector3Int pos)
    {
        if(pos.x < _groundTileMap.cellBounds.xMin || pos.x >= _groundTileMap.cellBounds.xMax 
            || pos.y < _groundTileMap.cellBounds.yMin || pos.y >= _groundTileMap.cellBounds.yMax)
        {
            //맵의 범위를 벗어남
            return false;
        }

        //해당 타일이 비어있으면 
        return _colliderTileMap.GetTile(pos) == null; 
    }
}
