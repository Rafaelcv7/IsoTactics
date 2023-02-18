using System;
using UnityEngine;

namespace IsoTactics.Stats
{
    public class MagicPoints : MonoBehaviour
    {
        public int maxMP;
        public int currentMP;

        private void Start()
        {
            maxMP = gameObject.GetComponent<Character>().wisdom /2;
            currentMP = maxMP;
        }

        public void Consume(int quantity)
        {
            if(currentMP == 0) return;
            currentMP -= quantity;
        }

        public void Restore(int quantity)
        {
            currentMP += quantity;
            if (currentMP > maxMP)
            {
                currentMP = maxMP;
            };
            
        }
    }
}