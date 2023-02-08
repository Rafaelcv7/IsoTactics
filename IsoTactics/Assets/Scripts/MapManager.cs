using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoTactics
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        public GameObject overlayPrefab;
        public GameObject overlayContainer;

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
            
            var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
            Map = new Dictionary<Vector2Int, OverlayTile>();

            foreach (var tm in tileMaps)
            {
                BoundsInt bounds = tm.cellBounds;

                for (var z = bounds.max.z; z > bounds.min.z; z--)
                {
                    for (var y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (var x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            if (tm.HasTile(new Vector3Int(x, y, z)))
                            {
                                if (!Map.ContainsKey(new Vector2Int(x, y)))
                                {
                                    var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                    var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder;
                                    overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);
    
                                    Map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                                }
                            }
                        }
                    }
                }
            }
        }

        //Currently only works with 1 characterJumpHeight
        public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile, int characterJumpHeight)
        {
            var surroundingTiles = new List<OverlayTile>();

            var tileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= characterJumpHeight)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= characterJumpHeight)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= characterJumpHeight)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            tileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (Map.ContainsKey(tileToCheck))
            {
                if (Mathf.Abs(Map[tileToCheck].transform.position.z - Map[originTile].transform.position.z) <= characterJumpHeight)
                    surroundingTiles.Add(Map[tileToCheck]);
            }

            return surroundingTiles;
        }
    }
}
