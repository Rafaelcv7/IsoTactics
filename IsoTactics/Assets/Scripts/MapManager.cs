using System.Collections.Generic;
using System.Linq;
using IsoTactics.Enums;
using UnityEngine;
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
        public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();

            var tileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (Map.ContainsKey(tileToCheck) && !Map[tileToCheck].isBlocked)
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (Map.ContainsKey(tileToCheck) && !Map[tileToCheck].isBlocked)
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (Map.ContainsKey(tileToCheck) && !Map[tileToCheck].isBlocked)
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (Map.ContainsKey(tileToCheck) && !Map[tileToCheck].isBlocked)
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            return surroundingTiles;
        }
        
        //TODO: REWORK
        public List<OverlayTile> GetAttackTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();

            var tileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            return surroundingTiles;
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

        public void ClearMapTiles()
        {
            foreach(var overlay in Instance.Map)
            {
                overlay.Value.HideTile();
            }
        }
    }
}
