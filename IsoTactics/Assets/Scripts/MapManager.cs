using System.Collections.Generic;
using System.Linq;
using IsoTactics.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using TileData = IsoTactics.TileConfig.TileData;

namespace IsoTactics
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        public GameObject overlayPrefab;
        public GameObject overlayContainer;
        public List<TileData> tileDatas;
        public Dictionary<TileBase, TileData> TexturesToType = new ();
        public Dictionary<Vector2Int, OverlayTile> Map;
        Dictionary<string, bool> directions;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else
            {
                Instance = this;
            }

        }

        void Start()
        {
            MapTextureToType(tileDatas);
            
            var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
            Map = new Dictionary<Vector2Int, OverlayTile>();

            foreach (var tilemap in tileMaps)
            {
                BoundsInt bounds = tilemap.cellBounds;

                for (var z = bounds.max.z; z > bounds.min.z; z--)
                {
                    for (var y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (var x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            var tileLocation = new Vector3Int(x, y, z);
                            var tileKey = new Vector2Int(x, y);
                            if (tilemap.HasTile(tileLocation) && !Map.ContainsKey(tileKey))
                            {
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform).GetComponent<OverlayTile>();
                                var cellWorldPosition = tilemap.GetCellCenterWorld(tileLocation);
                                var baseTile = tilemap.GetTile(tileLocation);
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                                overlayTile.gridLocation = tileLocation;

                                if (TexturesToType.ContainsKey(baseTile))
                                {
                                    overlayTile.tileData = TexturesToType[baseTile];
                                    overlayTile.isBlocked = TexturesToType[baseTile].type == TileTypes.NonTraversable;
                                }
                                
                                Map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                            }
                        }
                    }
                }
            }
        }

        //Currently only works with 1.
        public List<OverlayTile> GetSurroundingTiles(OverlayTile originTile, List<OverlayTile> searchableTiles, bool ignoreObstacles = false, bool walkThroughAllies = false)
        {
            var tilesToSearch = new Dictionary<Vector2Int, OverlayTile>();

            if (searchableTiles.Count > 0)
            {
                foreach (var tile in searchableTiles)
                {
                    tilesToSearch.Add(tile.Grid2DLocation, tile);
                }
            }
            else
            {
                tilesToSearch = Map;
            }

            var neighbours = new List<OverlayTile>();
            if (originTile != null)
            {
                //Right
                var tileToCheck = new Vector2Int(originTile.Grid2DLocation.x +1, originTile.Grid2DLocation.y);
                ValidateNeighbour(originTile, ignoreObstacles, walkThroughAllies, tilesToSearch, neighbours, tileToCheck);

                //Left
                tileToCheck = new Vector2Int(originTile.Grid2DLocation.x -1, originTile.Grid2DLocation.y);
                ValidateNeighbour(originTile, ignoreObstacles, walkThroughAllies, tilesToSearch, neighbours, tileToCheck);

                //Up
                tileToCheck = new Vector2Int(originTile.Grid2DLocation.x, originTile.Grid2DLocation.y +1);
                ValidateNeighbour(originTile, ignoreObstacles, walkThroughAllies, tilesToSearch, neighbours, tileToCheck);

                //Down
                tileToCheck = new Vector2Int(originTile.Grid2DLocation.x, originTile.Grid2DLocation.y -1);
                ValidateNeighbour(originTile, ignoreObstacles, walkThroughAllies, tilesToSearch, neighbours, tileToCheck);
            }

            return neighbours;
        }
        
        //TODO: REWORK
        public List<OverlayTile> GetAttackTiles(OverlayTile originTile, OverlayTile characterActiveTile)
        {
            var neighbours = new List<OverlayTile>();
            var tilesToSearch = Instance.Map;

            //Right
            var tileToCheck = new Vector2Int(originTile.Grid2DLocation.x + 1, originTile.Grid2DLocation.y);
            ValidateNeighbourAttack(originTile, tilesToSearch, neighbours, tileToCheck);

            //Left
            tileToCheck = new Vector2Int(originTile.Grid2DLocation.x - 1, originTile.Grid2DLocation.y);
            ValidateNeighbourAttack(originTile, tilesToSearch, neighbours, tileToCheck);

            //Up
            tileToCheck = new Vector2Int(originTile.Grid2DLocation.x, originTile.Grid2DLocation.y + 1);
            ValidateNeighbourAttack(originTile, tilesToSearch, neighbours, tileToCheck);

            //Down
            tileToCheck = new Vector2Int(originTile.Grid2DLocation.x, originTile.Grid2DLocation.y - 1);
            ValidateNeighbourAttack(originTile, tilesToSearch, neighbours, tileToCheck);

            return neighbours;
        }

        private void MapTextureToType(List<TileData> tileDatas)
        {
            if (tileDatas.Count > 0)
            {
                foreach (var tileData in tileDatas)
                {
                    foreach (var texture in tileData.tilesTextures)
                    {
                        TexturesToType.Add(texture, tileData);
                    }
                }
            }
        }

        private void ValidateNeighbourAttack(OverlayTile currentOverlayTile, Dictionary<Vector2Int, OverlayTile> tilesToSearch, List<OverlayTile> neighbours, Vector2Int locationToCheck)
        {
            if (tilesToSearch.ContainsKey(locationToCheck))
            {
                if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 2)
                    neighbours.Add(tilesToSearch[locationToCheck]);
            }
        }
        
        private static void ValidateNeighbour(OverlayTile currentOverlayTile, bool ignoreObstacles, bool walkThroughAllies, Dictionary<Vector2Int, OverlayTile> tilesToSearch, List<OverlayTile> neighbours, Vector2Int locationToCheck)
        {
            if (tilesToSearch.ContainsKey(locationToCheck) &&
                (ignoreObstacles ||
                 (!tilesToSearch[locationToCheck].isBlocked) ||
                 (walkThroughAllies &&
                  (tilesToSearch[locationToCheck].activeCharacter))))
            {
                if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 1)
                    neighbours.Add(tilesToSearch[locationToCheck]);
            }
        }

        public void ClearMapTiles()
        {
            foreach(var overlay in Instance.Map)
            {
                overlay.Value.HideTile();
            }
        }
    }
}
