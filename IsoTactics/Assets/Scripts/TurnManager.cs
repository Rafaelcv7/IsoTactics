using System.Collections.Generic;
using UnityEngine;

namespace IsoTactics
{
    public class TurnManager : MonoBehaviour
    {
        public List<Character> charactersContainer; 
        public int currentTurn;
        
        private Character _activeCharacter;
        private int _characterNum;

        [Header("Events")] public GameEvents onNewActiveCharacter;
        private void Start()
        {
            currentTurn = 0;
            charactersContainer = new List<Character>();
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
            _activeCharacter.RestartPTStats();
            
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
            if (data is Character newCharacter)
            {
                charactersContainer.Add(newCharacter);
            }
        }
        
        //Called By Character -> Health.
        public void KillCharacter(Component sender, object data)
        {
            if (data is Character deathCharacter)
            {
                charactersContainer.Remove(deathCharacter);
                _characterNum = charactersContainer.Count;
                currentTurn = (currentTurn + 1) % _characterNum;
            }
        }
    }
}