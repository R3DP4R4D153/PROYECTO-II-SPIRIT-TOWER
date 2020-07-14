﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SignalCreator : ScriptableObject
{
    public List<SignalListener> Listeners = new List<SignalListener>();

    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].OnSignalRaise();
        }
    }

    public void RegisterListener(SignalListener listener)
    {
        Listeners.Add(listener);
    }

    public void DeRegisterListener(SignalListener listener)
    {
        Listeners.Remove(listener);
    }
}