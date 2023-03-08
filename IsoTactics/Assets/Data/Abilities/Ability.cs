using System.Collections.Generic;
using UnityEngine;

namespace IsoTactics.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        public int abilityRange;
        public Sprite abilityIcon;
        public BaseCharacter character;
        public int cost;
        public int cooldown;
        
        private readonly RangeFinder _rangeFinder = new ();

        public virtual void Execute(OverlayTile tile){}

        public List<OverlayTile> GetAbilityRange(BaseCharacter character)
        {
            return _rangeFinder.GetTilesInAbilityRange(
                character,
                abilityRange
            );
        }
    }
}