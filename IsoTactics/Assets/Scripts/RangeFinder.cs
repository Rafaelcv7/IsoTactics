using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IsoTactics
{
    public class RangeFinder
    {
        public List<OverlayTile> GetTilesInRange(Vector2Int location, int characterMovementPoints)
        {
            var startingTile = MapManager.Instance.Map[location];
            var inRangeTiles = new List<OverlayTile>();
            int stepCount = 0;

            inRangeTiles.Add(startingTile);

            //Should contain the surroundingTiles of the previous step. 
            var tilesForPreviousStep = new List<OverlayTile> { startingTile };
            while (stepCount < characterMovementPoints)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(MapManager.Instance.GetSurroundingTiles(new Vector2Int(item.gridLocation.x, item.gridLocation.y)));
                }

                inRangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeTiles.Distinct().ToList();
        }
        
        //TODO: REWORK
        public List<OverlayTile> GetTilesInAbilityRange(Vector2Int location, int abilityRange)
        {
            var startingTile = MapManager.Instance.Map[location];
            var inRangeTiles = new List<OverlayTile>();
            int stepCount = 0;

            inRangeTiles.Add(startingTile);

            //Should contain the surroundingTiles of the previous step. 
            var tilesForPreviousStep = new List<OverlayTile> { startingTile };
            while (stepCount < abilityRange)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(MapManager.Instance.GetAttackTiles(new Vector2Int(item.gridLocation.x, item.gridLocation.y)));
                }

                inRangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeTiles.Distinct().ToList();
        }
    }
}
