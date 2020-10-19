
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;
    public float nearbyEventRadius = 10f;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TriggerRandomEvent();
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
    public static void TriggerRandomEvent()
    {
        List<EventBase> events = EventBase.events;
        EventBase eventFound = events[Random.Range(0, events.Count - 1)];
        eventFound.EventTrigger();
    }

    //Trigger a list of events
    public static void TriggerEvents(EventBase[] events)
    {
        foreach(EventBase selectedEvent in events)
        {
            selectedEvent.EventTrigger();
        }
    }
}
