using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventTimedBase : EventBase
{
    [Range(0f,60f)]
    public float eventTime = 0;

    private Coroutine timedEvent;

    //Called when an event wants to be triggered
    public override bool OnEventTrigger()
    {
        if (CanTrigger)
        {
            if (!MultipleTriggers)
                HasTriggered = true;
            else
                StartCoroutine(EventCoolDown());

            EventTrigger();
            CanTrigger = false;

            return true;
        }

        return false;
    }

    //Called if the event can be triggered
    public override void EventTrigger()
    {
        //Trigger timed event
        timedEvent = StartCoroutine(TimedEvent());

        //wait for timed event to finish to trigger
        StartCoroutine(WaitEventCompletion());
    }

    //Called to perform actual timed event
    public abstract IEnumerator TimedEvent();

    //Called when TimedEvent is finished
    public IEnumerator WaitEventCompletion()
    {
        yield return timedEvent;

        if (HasTriggered)
            Destroy(gameObject);
    }
}
