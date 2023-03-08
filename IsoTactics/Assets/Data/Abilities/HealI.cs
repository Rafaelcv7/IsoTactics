using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoTactics.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Heal_I")]
    public class HealI : Ability
    {
        public override void Execute(OverlayTile tile)
        {
            var healing = (int)(character.Stats.intelligence.statValue + (character.Stats.wisdom.statValue /2f));
            tile.activeCharacter.HP.Heal(healing);
        }
    }
}