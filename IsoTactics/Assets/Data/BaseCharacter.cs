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
        public bool isAlive;
        public StateManager State => gameObject.GetComponent<StateManager>();

        [Header("Stats:")]
        public CharacterClass characterClass;
        public Health HP;
        public MagicPoints MP;
        public CharacterStats Stats;


        protected void SetStats()
        {
            Stats.strength = new Stat(Enums.Stats.Strength, characterClass.strength, this);
            Stats.vitality = new Stat(Enums.Stats.Vitality, characterClass.vitality, this);
            Stats.accuracy = new Stat(Enums.Stats.Accuracy, characterClass.accuracy, this);
            Stats.agility = new Stat(Enums.Stats.Agility, characterClass.agility, this);
            Stats.intelligence = new Stat(Enums.Stats.Intelligence, characterClass.intelligence, this);
            Stats.wisdom = new Stat(Enums.Stats.Wisdom, characterClass.wisdom, this);
            Stats.resistance = new Stat(Enums.Stats.Resistance, characterClass.resistance, this);
            Stats.actionPoints = new Stat(Enums.Stats.ActionPoints, characterClass.actionPoints, this);
            Stats.movementPoints = new Stat(Enums.Stats.MovementPoints, characterClass.movementPoints, this);
        }

        public void LinkCharacterToTile(OverlayTile tile)
        {
            UnlinkCharacterToTile();
            tile.activeCharacter = this as Character;
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