using UnityEngine;

namespace IsoTactics.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CharacterClass")]
    public class CharacterClass : ScriptableObject
    {
        public int strength;        //Affects damage dealt with PHYSICAL attacks.
        public int vitality;        //Affects DEFENSE against attacks & Health pool.
        public int accuracy;        //Affects CHANCES of landing successful attack.
        public int agility;         //Affects CHANCES of avoiding a attack and the POSITION on the turn manager.
        public int intelligence;    //Affects the strength of OFFENSIVE magic.
        public int wisdom;          //Affects CHANCES of offensive and defensive magic & your resistance to it.
        public int resistance;      //Affects global resistance to effects and elemental attacks.
        public int actionPoints;    //How many actions can this character do per turn. 
        public int movementPoints;  //How many spaces may this character move per turn.
    }
}