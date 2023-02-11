using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class TurnManager : MonoBehaviour
    {
        public List<CharacterInfo> charactersContainer; 
        public int currentTurn;
        
        private CharacterInfo _activeCharacter;
        private int _characterNum;

        [Header("Events")] public GameEvents onNewActiveCharacter;
        private void Start()
        {
            currentTurn = 0;
            charactersContainer = new List<CharacterInfo>();
            EvaluatePhase();
        }

        private void EvaluatePhase()
        {
            switch (GamePhases.CurrentPhase)
            {
                case "Positioning":
                {
                    Invoke(nameof(PositioningPhase), 0);
                    break; 
                }
                case "Turn":
                {
                    Invoke(nameof(TurnPhase), 2f); 
                    break;
                }
            }
        }


        private void PositioningPhase()
        {
            
        }

        private void TurnPhase()
        {
            _characterNum = charactersContainer.Count;
            _activeCharacter = charactersContainer[currentTurn];
            _activeCharacter.RestartStats();
            
            onNewActiveCharacter.Raise(this, _activeCharacter);
        }

        //Called by MovementController.
        public void PassTurn(Component sender, object data)
        {
            currentTurn = (currentTurn + 1) % _characterNum;
            EvaluatePhase();
        }

        //Called by Spawner.
        public void ChangePhase(Component sender, object data)
        {
            if (data is string phase)
            {
                GamePhases.ChangeCurrentPhase(phase);  
                EvaluatePhase();
            }
        }

        //Called by Spawner.
        public void AddNewCharacter(Component sender, object data)
        {
            if (data is CharacterInfo newCharacter)
            {
                charactersContainer.Add(newCharacter);
            }
        }
    }
}