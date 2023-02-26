using System;
using System.Collections.Generic;
using IsoTactics.Abilities;
using UnityEngine;

namespace IsoTactics
{
    public class Character : BaseCharacter
    {
        [Header("Info:")]
        public string Name;
        public Sprite portrait;
        [Header("Abilities:")]
        public List<Ability> abilities;

        private void Start()
        {
            if (characterClass) { SetStats(); }
            else 
            {
                throw new Exception(
                    "Character does not have a class assigned. Please assign a class to this character.");
            }

            List<Ability> abilitiesPouch = new ();
            abilities.ForEach(x =>
                {
                    if (x)
                    {
                        x = Instantiate(x);
                        x.character = this;
                        abilitiesPouch.Add(x);
                    }
                }
            );
            abilities = abilitiesPouch;
        }

        public void RestartPTStats()
        {
            Stats.actionPoints.statValue = Stats.actionPoints.baseStat;
            Stats.movementPoints.statValue = Stats.movementPoints.baseStat;
        }
    }
}
