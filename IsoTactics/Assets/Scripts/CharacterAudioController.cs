using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterAudioController : MonoBehaviour
{
    public BaseCharacter character;
    public AudioSource audioController;
    public StateManager stateManager;


    private void Start()
    {
        character = gameObject.GetComponentInParent<BaseCharacter>();
        stateManager = gameObject.GetComponentInParent<StateManager>();
    }

    void Update()
    {
        if (stateManager.animator.GetBool(stateManager.IsWalking))
        {
            audioController.UnPause();
        }
        else
        {
            audioController.Pause();
        }
    }
}
