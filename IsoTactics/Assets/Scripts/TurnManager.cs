using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class TurnManager : MonoBehaviour
    {
        public List<GameObject> charactersPrefabs;
        public List<CharacterInfo> charactersContainer; 
        public int currentTurn;
        public MouseController controller;
        private CharacterInfo currentC;
        public bool onPosPhase;
        private int _characterNum;

        private void Start()
        {
            onPosPhase = true;
            currentTurn = 0;
            charactersContainer = new List<CharacterInfo>();
            _characterNum = charactersPrefabs.Count;
            PositioningPhase();
        }
        

        private void PositioningPhase()
        {
            var current = charactersPrefabs[0];
            

            controller.characterPrefab = current;
            
        }

        private void TurnPhase()
        {
            controller.characterPrefab = null;
            
            controller.StartTurn(charactersContainer[currentTurn]);
        }

        public void PassTurn()
        {
            currentTurn = (currentTurn + 1) % _characterNum;
            if (onPosPhase) {charactersPrefabs.RemoveAt(0);}
            if (charactersPrefabs.Count != 0)
            {
                Invoke(nameof(PositioningPhase), 0);
            }
            else
            {
                onPosPhase = false;
                Invoke(nameof(TurnPhase), 2f);  
            }
        }

        private void TakeTurn()
        {
            GameObject current = charactersPrefabs[currentTurn];

            controller.characterPrefab = current;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentC.RestartStats(); 
                currentTurn = (currentTurn + 1) % _characterNum;
                Invoke(nameof(TakeTurn), 2f);  
            }
        }
    }
}