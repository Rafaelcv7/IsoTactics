using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class CharacterInfo : MonoBehaviour
    {
        [HideInInspector]
        public OverlayTile standingOnTile;

        public Health health;
        public int speed;
        public int movementSpeed;
        public int jumpHeight;
        public int movementPoints;

        private void Start()
        {
            health.maxHealth = 25 * 2; //vitality * 2
        }

        public void RestartStats()
        {
            movementSpeed = (int)(10f / 100f* speed);
            movementPoints = (int)(speed / 5f);
        }
    }
}
