using System.Collections.Generic;
using UnityEngine;

namespace IsoTactics.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        public int abilityRange;
        public Sprite abilityIcon;
        
        private readonly RangeFinder _rangeFinder = new ();

        public virtual void Execute(OverlayTile tile){}

        public List<OverlayTile> GetAbilityRange(BaseCharacter character)
        {
            return _rangeFinder.GetTilesInAbilityRange(
                new Vector2Int(character.activeTile.Grid2DLocation.x, character.activeTile.Grid2DLocation.y),
                abilityRange
            );
        }
    }
}