using System;
using UnityEngine.Serialization;

namespace IsoTactics.Stats
{
    [Serializable]
    public class Stat
    {
        public Enums.Stats statKey;

        public int baseStat;
        public int statValue;

        public Stat(Enums.Stats statKey, int statValue, BaseCharacter character)
        {
            this.statValue = statValue;
            this.statKey = statKey;

            baseStat = statValue;
        }

        public void ChangeStatValue(int newValue)
        {
            statValue = newValue;
            
        }
    }
}