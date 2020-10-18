using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTimedBase : EventBase
{
    public float eventTime;

    public override void OnEventTrigger()
    {
        base.OnEventTrigger();
        StartCoroutine(TimedEvent());
    }

    public abstract IEnumerator TimedEvent();
}
