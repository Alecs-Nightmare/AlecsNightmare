using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/GameEvent")]
public class GameEvent : ScriptableObject {

    private List<GameEventListener> listeners = new List<GameEventListener>();

	public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void UnregisterListener(GameEventListener gameEventListener)
    {
        listeners.Remove(gameEventListener);
    }

    public void RegisterListener(GameEventListener gameEventListener)
    {
        listeners.Add(gameEventListener);
    }
}
