using System;
using UnityEngine;

namespace IsoTactics
{
    public class TurnManager : MonoBehaviour
    {
        public GameObject[] combatantObjects;
        private int _currentCombatant;

        private void Start()
        {
            _currentCombatant = 0;
            TakeTurn();
        }

        private void TakeTurn()
        {
            GameObject current = combatantObjects[_currentCombatant];
            
            //Add Logic to Perform Turn Actions for the currentCombatant

            _currentCombatant = (_currentCombatant + 1) % combatantObjects.Length;
            Invoke("TakeTurn", 2f);
        }
    }
}