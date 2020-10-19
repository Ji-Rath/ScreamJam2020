using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTimedBase : EventBase
{
    [Range(0f,60f)]
    public float eventTime = 0;

    public override void OnEventTrigger()
    {
        StartCoroutine(TimedEvent());
    }

    public abstract IEnumerator TimedEvent();
}
