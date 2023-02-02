using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Instance => _instance;

    private readonly float _offSetY = 0.16f;

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();
        var tileMaps = gameObject.GetComponentsInChildren<Tilemap>();
        
        foreach (var tileMap in tileMaps)
        {
            if (tileMap.GetComponent<LayerProperties>().IsWalkable)
            {
                BoundsInt bounds = tileMap.cellBounds;

                for (int z = bounds.max.z; z >= bounds.min.z; z--)
                {
                    for (var x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        for (var y = bounds.min.y; y < bounds.max.y; y++)
                        {
                            var tileLocation = new Vector3Int(x, y, z);

                            var tileKey = new Vector2Int(x, y);

                            if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                            {
                                var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                                var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y + _offSetY, cellWorldPosition.z -1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                                    tileMap.GetComponent<TilemapRenderer>().sortingOrder +1;
                                overlayTile.gridLocation = tileLocation;
                                map.Add(tileKey, overlayTile);
                            }
                        }
                    }
                } 
            }
        }
    }
    
    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
    {
        var InRangeTiles = new Dictionary<Vector2Int, OverlayTile>();

        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                InRangeTiles.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            InRangeTiles = map;
        }
        
        var neighbours = new List<OverlayTile>();

        //top
        var locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y +1
        );

        if (InRangeTiles.ContainsKey(locationToCheck))
        {
            if(Mathf.Abs(currentOverlayTile.gridLocation.z - InRangeTiles[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(InRangeTiles[locationToCheck]); 
        }
        
        //bottom
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y -1
        );

        if (InRangeTiles.ContainsKey(locationToCheck))
        {
            if(Mathf.Abs(currentOverlayTile.gridLocation.z - InRangeTiles[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(InRangeTiles[locationToCheck]);  
        }
        
        //right
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x +1,
            currentOverlayTile.gridLocation.y
        );

        if (InRangeTiles.ContainsKey(locationToCheck))
        {
            if(Mathf.Abs(currentOverlayTile.gridLocation.z - InRangeTiles[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(InRangeTiles[locationToCheck]); 
        }
        
        //left
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x -1,
            currentOverlayTile.gridLocation.y
        );

        if (InRangeTiles.ContainsKey(locationToCheck))
        {
            if(Mathf.Abs(currentOverlayTile.gridLocation.z - InRangeTiles[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(InRangeTiles[locationToCheck]); 
        }

        return neighbours;
    }
}
