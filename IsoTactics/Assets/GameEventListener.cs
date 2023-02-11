using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameEventListener : MonoBehaviour
{
    [System.Serializable]
    public class CustomGameEvent : UnityEvent<Component, object> {}
    
    public GameEvents gameEvent;

    public CustomGameEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }

    private void OnDestroy()
    {
        gameEvent.listeners = new List<GameEventListener>();
    }
}
