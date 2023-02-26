using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsoTactics.Abilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonEventHandler : MonoBehaviour
{
    public Ability ability;
    [Header("Event:")] 
    public List<GameEvents> onClickEvents;

    private Image _selected;

    private void Start()
    {
        _selected = gameObject.GetComponentsInChildren<Image>()?.First(x => x.name == "Select");
        _selected?.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        _selected.gameObject.SetActive(!_selected.gameObject.activeSelf);
        if(onClickEvents.Count > 0)
            onClickEvents.ForEach(x => x.Raise(this, ability));
        gameObject.GetComponent<AudioSource>().Play();
    }
}
