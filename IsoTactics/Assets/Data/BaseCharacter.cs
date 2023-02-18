using System;
using IsoTactics;
using IsoTactics.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    [RequireComponent(typeof(StateManager))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(MagicPoints))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public OverlayTile activeTile;
        public StateManager State => gameObject.GetComponent<StateManager>();

        [Header("Stats:")]
        public int movementPoints;
        public Health HP;
        public MagicPoints MP;
        public int strength;
        public int speed = 3;
        public int wisdom;
        public int vitality;


        public void LinkCharacterToTile(OverlayTile tile)
        {
            UnlinkCharacterToTile();
            tile.activeCharacter = this;
            tile.isBlocked = true;
            activeTile = tile;
        }

        public void UnlinkCharacterToTile()
        {
            if(!activeTile) return;

            activeTile.activeCharacter = null;
            activeTile.isBlocked = false;
            activeTile = null;
        }
    }
}