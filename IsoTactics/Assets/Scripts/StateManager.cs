using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using UnityEngine;
using UnityEngine.Serialization;

namespace IsoTactics
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class StateManager : MonoBehaviour
    {
        public Animator animator;
        public readonly int IsWalking = Animator.StringToHash("IsWalking");
        public readonly int FacingDirection = Animator.StringToHash("FacingDirection");
        public readonly int IsDeath = Animator.StringToHash("Death");
        private SpriteRenderer _characterRender;
        private Character _character;
        private bool _toDirection;
        private CharacterAudioController _audioController;

        private void Start()
        {
            _character = gameObject.GetComponent<Character>();
            _characterRender = _character.GetComponent<SpriteRenderer>();
            _audioController = _character.GetComponentInChildren<CharacterAudioController>();
        }

        public void EvaluateMovingState(OverlayTile tile, bool isMoving)
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

        public void ToDeathState()
        {
            animator.SetBool(IsDeath, true);
        }
    } 
}

