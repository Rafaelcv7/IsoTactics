using System;
using System.Collections.Generic;
using IsoTactics.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class Character : BaseCharacter
    {
        [Header("Info:")]
        public string Name;
        public Sprite portrait;
        [Header("Abilities:")]
        public List<Ability> abilities;

        public void RestartStats()
        {
            movementPoints = 4;
        }
    }
}
