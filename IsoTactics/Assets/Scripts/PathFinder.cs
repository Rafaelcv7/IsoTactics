using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IsoTactics
{
    public class PathFinder
    {
        private Dictionary<Vector2Int, OverlayTile> searchableTiles;

        public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> tilesToSearch, bool ignoreObstacles = false, bool walkTroughAllies = false)
        {
            var openList = new List<OverlayTile>();
            var closedList = new HashSet<OverlayTile>();

            openList.Add(start);

            while (openList.Count > 0)
            {
                var currentOverlayTile = openList.OrderBy(x => x.F).First();

                openList.Remove(currentOverlayTile);
                closedList.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    return GetFinishedList(start, end);
                }

                var neighbourTiles = MapManager.Instance.GetSurroundingTiles(currentOverlayTile, tilesToSearch, ignoreObstacles, walkTroughAllies);

                foreach (var tile in neighbourTiles)
                {
                    if (closedList.Contains(tile))
                    {
                        continue;
                    }

                    tile.G = GetManhattenDistance(start, tile);
                    tile.H = GetManhattenDistance(end, tile);

                    tile.previousTile = currentOverlayTile;


                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }
            }

            return new List<OverlayTile>();
        }

        private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> finishedList = new List<OverlayTile>();
            OverlayTile currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.previousTile;
            }

            finishedList.Reverse();

            return finishedList;
        }

        public int GetManhattenDistance(OverlayTile start, OverlayTile tile)
        {
            return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
        }

    }
}