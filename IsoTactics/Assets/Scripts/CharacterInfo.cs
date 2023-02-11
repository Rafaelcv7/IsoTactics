using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class CharacterInfo : MonoBehaviour
    {
        [HideInInspector]
        public OverlayTile activeTile;
        

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
        
        //When an Entity moves, link it to the tiles it's standing on. 
        public void LinkCharacterToTile(OverlayTile tile)
        {
            UnlinkCharacterToTile();
            tile.activeCharacter = this;
            tile.isBlocked = true;
            activeTile = tile;
        }

        //Unlink an entity from a previous tile it was standing on. 
        public void UnlinkCharacterToTile()
        {
            if (activeTile)
            {
                activeTile.activeCharacter = null;
                activeTile.isBlocked = false;
                activeTile = null;
            }
        }
    }
}
