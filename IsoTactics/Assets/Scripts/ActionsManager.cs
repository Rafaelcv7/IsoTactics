using System;
using System.Collections.Generic;
using IsoTactics.Abilities;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    public class ActionsManager : MonoBehaviour
    {
        public Character activeCharacter;
        public GameEvents toggleMovement;
        private List<OverlayTile> _inAttackRangeTiles;
        private OverlayTile _tile;
        private Ability _ability;
        private bool _onAbilityAction;
        

        private void Update()
        {
            if (_onAbilityAction && _tile && activeCharacter.Stats.actionPoints.statValue > 0)
            {
                _inAttackRangeTiles = _ability.GetAbilityRange(activeCharacter);
                ShowRange();

                if (_inAttackRangeTiles.Contains(_tile))
                {
                    _tile.GetComponent<SpriteRenderer>().color = Color.red;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (_tile.activeCharacter && _inAttackRangeTiles.Contains(_tile))
                    {
                        activeCharacter.State.EvaluateMovingState(_tile, false);
                        _ability.Execute(_tile);
                        _onAbilityAction = false;
                        HideRange();
                        if(toggleMovement){toggleMovement.Raise(this, null);}
                        activeCharacter.Stats.actionPoints.statValue--;
                    }
                }
            }
            else
            {
                HideRange();
            }
        }
        
        
        private void ShowRange()
        {
            _inAttackRangeTiles?.ForEach(x => x.ShowTile());
        }

        private void HideRange()
        {
            _inAttackRangeTiles?.ForEach(x => x.HideTile());
        }


        //Called by Mouse Controller.
        public void NewFocusedTile(Component sender, object data)
        {
            if (data is OverlayTile tile)
            {
                _tile = tile;
            }
        }
        //Called by TurnManager
        public void SetActiveCharacter(Component sender, object data)
        {
            if (data is Character newCharacter)
            {
                activeCharacter = newCharacter;
            }
        }
        
        //Called by Attack UI Button.
        public void Attack(Component sender, object data)
        {
            if (data is Ability ability)
            {
                _onAbilityAction = !_onAbilityAction;
                _ability = ability;
            }
            
        }
    }
}