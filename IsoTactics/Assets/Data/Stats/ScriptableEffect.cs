using IsoTactics.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics.Stats
{
    //ScriptableEffects can be attached to both tiles and abilities. 
    [CreateAssetMenu(fileName = "ScriptableEffect", menuName = "ScriptableObjects/ScriptableEffect")]
    public class ScriptableEffect : ScriptableObject
    {
        public Enums.Stats statKey;
        public Operation @operator;
        public float duration;
        public int value;

        public Enums.Stats GetStatKey()
        {
            return statKey;
        }
    }
}