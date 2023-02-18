using System.Collections;
using System.Collections.Generic;
using IsoTactics.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonEventHandler : MonoBehaviour
{
    public Ability ability;
    [Header("Event:")] 
    public List<GameEvents> onClickEvents;

    public void OnButtonClick()
    {
        if(onClickEvents.Count > 0)
            onClickEvents.ForEach(x => x.Raise(this, ability));
        gameObject.GetComponent<AudioSource>().Play();
    }
}
