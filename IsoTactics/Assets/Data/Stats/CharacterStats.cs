using System.Linq;
using UnityEngine;

namespace IsoTactics.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public Stat strength;        //Affects damage dealt with PHYSICAL attacks.
        public Stat vitality;        //Affects DEFENSE against attacks & Health pool.
        public Stat accuracy;        //Affects CHANCES of landing successful attack.
        public Stat agility;         //Affects CHANCES of avoiding a attack and the POSITION on the turn manager.
        public Stat intelligence;    //Affects the strength of OFFENSIVE magic.
        public Stat wisdom;          //Affects CHANCES of offensive and defensive magic & your resistance to it.
        public Stat resistance;      //Affects global resistance to effects and elemental attacks.
        public Stat actionPoints;    //How many actions can this character do per turn. 
        public Stat movementPoints;  //How many spaces may this character move per turn.

        public Stat GetStat(Enums.Stats statKey)
        {
            var fields = typeof(CharacterStats).GetFields().ToList();

            return fields.Select(item => (Stat)item.GetValue(this)).FirstOrDefault(value => value.statKey == statKey);
        }
    }
}