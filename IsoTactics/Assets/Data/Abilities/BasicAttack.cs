using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/BasicAttack")]
    public class BasicAttack : Ability
    {
        public override void Execute(OverlayTile tile)
        {
            var damage = character.Stats.strength.statValue;
            tile.activeCharacter.HP.TakeDamage(damage, character.Stats.accuracy.statValue);
        }
    }
}