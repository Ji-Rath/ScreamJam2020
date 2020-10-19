using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;
    public float nearbyEventRadius = 10f;
    private EventBase[] events;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Get all objects with EventBase and store them
        events = FindObjectsOfType<EventBase>();
    }

    //Get a random nearby event and trigger it
    public void TriggerNearbyEvent()
    {
        LayerMask mask = LayerMask.GetMask("Events");
        Collider[] hitEvents = Physics.OverlapSphere(transform.position, nearbyEventRadius, mask);
        EventBase eventFound = hitEvents[Random.Range(0, hitEvents.Length - 1)].GetComponent<EventBase>();
        eventFound.OnEventTrigger();
    }

    //Trigger a random event
    public void TriggerRandomEvent()
    {
        EventBase eventFound = events[Random.Range(0, events.Length - 1)];
        eventFound.OnEventTrigger();
    }

    //Trigger a list of events
    public void TriggerEvents(EventBase[] events)
    {
        foreach(EventBase selectedEvent in events)
        {
            selectedEvent.OnEventTrigger();
        }
    }
}
