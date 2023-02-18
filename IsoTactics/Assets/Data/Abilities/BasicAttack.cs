using UnityEngine;

namespace IsoTactics.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/BasicAttack")]
    public class BasicAttack : Ability
    {
        public override void Execute(OverlayTile tile)
        {
            tile.activeCharacter.HP.TakeDamage(10);
        }
    }
}