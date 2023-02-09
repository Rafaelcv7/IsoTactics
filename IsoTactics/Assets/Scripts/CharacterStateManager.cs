using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using UnityEngine;
using UnityEngine.Serialization;
using CharacterInfo = IsoTactics.CharacterInfo;

namespace IsoTactics
{
    public class CharacterStateManager : MonoBehaviour
    {
        public Animator animator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int FacingDirection = Animator.StringToHash("FacingDirection");
        private SpriteRenderer _characterRender;
        private CharacterInfo _character;
        private bool _toDirection;

        private void Start()
        {
            _character = gameObject.GetComponent<CharacterInfo>();
            _characterRender = _character.GetComponent<SpriteRenderer>();
        }

        public void EvaluateState(OverlayTile tile, bool isMoving)
        {
            if (tile != null)
            {
                _characterRender.flipX = tile.transform.position.x < _character.transform.position.x;
                _toDirection = tile.transform.position.y < _character.transform.position.y;
                animator.SetBool(FacingDirection, _toDirection);
            }
            else
            {
                animator.SetBool(FacingDirection, _toDirection);
            }
            animator.SetBool(IsWalking, isMoving);
        }
    } 
}

