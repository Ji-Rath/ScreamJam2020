using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPowerCut : EventBase
{
    public List<GameObject> lights;

    public override void OnEventTrigger()
    {
        base.OnEventTrigger();
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }
}
