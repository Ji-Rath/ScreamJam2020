using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase : MonoBehaviour
{
    [Tooltip("Whether the event can trigger multiple times")]
    public bool MultipleTriggers = false;
    [Tooltip("Whether the event can be triggered")]
    public bool CanTrigger = true;
    private bool HasTriggered = false;

    public virtual void OnEventTrigger()
    {
        HasTriggered = true;
    }
}
