
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
            TriggerRandomEvent();
    }

    //Get a random nearby event and trigger it
    public void TriggerNearbyEvent()
    {
        LayerMask mask = LayerMask.GetMask("Events");
        Collider[] hitEvents = Physics.OverlapSphere(transform.position, nearbyEventRadius, mask);
        if(hitEvents.Length > 0)
        {
            EventBase eventFound = hitEvents[Random.Range(0, hitEvents.Length)].GetComponent<EventBase>();

            //Just loop through the events if the first random one doesnt work, lol
            if (!eventFound.OnEventTrigger())
            {
                for (int i = 0; i < hitEvents.Length; i++)
                {
                    eventFound = hitEvents[i].GetComponent<EventBase>();
                    if (eventFound.OnEventTrigger())
                        break;
                }
            }
        }
    }

    //Trigger a random event
    public static void TriggerRandomEvent()
    {
        List<EventBase> events = EventBase.events;
        if (events.Count > 0)
        {
            EventBase eventFound = events[Random.Range(0, events.Count)];

            //Just loop through the events if the first random one doesnt work, lol
            if (!eventFound.OnEventTrigger())
            {
                for (int i = 0; i < events.Count; i++)
                {
                    eventFound = events[i].GetComponent<EventBase>();
                    if (eventFound.OnEventTrigger())
                        break;
                }
            }
        }
        
    }

    //Trigger a list of events
    public static void TriggerEvents(EventBase[] events)
    {
        foreach(EventBase selectedEvent in events)
        {
            selectedEvent.OnEventTrigger();
        }
    }
}
