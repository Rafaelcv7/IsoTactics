using System;
using System.Collections;
using System.Collections.Generic;
using IsoTactics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvents : ScriptableObject
{
    public List<GameEventListener> listeners = new();
    
    public void Raise(Component sender, object data)
    {
        listeners.ForEach(x => x.OnEventRaised(sender, data));
    }

    public void RegisterListener(GameEventListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
