using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToggleComponentTimed : EventTimedBase
{
    public List<Behaviour> toggleComponent = new List<Behaviour>();
    [Tooltip("Whether to make component disabled or enabled for a set time")]
    public bool SetDisabled;

    public override IEnumerator TimedEvent()
    {
        foreach(Behaviour component in toggleComponent)
            component.enabled = !SetDisabled;

        yield return new WaitForSeconds(eventTime);

        foreach (Behaviour component in toggleComponent)
            component.enabled = SetDisabled;
    }
}
