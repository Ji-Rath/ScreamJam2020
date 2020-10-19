using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToggleComponent : EventBase
{
    public List<Behaviour> toggleComponent = new List<Behaviour>();
    [Tooltip("Whether to make component disabled or enabled")]
    public bool SetDisabled;

    public override void OnEventTrigger()
    {
        foreach (Behaviour component in toggleComponent)
            component.enabled = !SetDisabled;
    }
}
