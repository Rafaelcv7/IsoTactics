using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class CharacterInfo : MonoBehaviour
    {
        public OverlayTile standingOnTile;
        [FormerlySerializedAs("MovementSpeed")] public int movementSpeed;
        [FormerlySerializedAs("JumpHeight")] public int jumpHeight;
        [FormerlySerializedAs("MovementPoints")] public int movementPoints;
        [FormerlySerializedAs("UpState")] public Sprite upState;
    }
}
