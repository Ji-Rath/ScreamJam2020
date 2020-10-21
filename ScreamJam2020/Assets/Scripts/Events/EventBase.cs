using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase : MonoBehaviour
{
    [Tooltip("Whether the event can trigger multiple times")]
    public bool MultipleTriggers = false;
    [Tooltip("Time between being able to trigger the event")]
    public float eventCooldown = 5f;
    [Tooltip("Whether the event can be triggered")]
    public bool CanTrigger = true;
    protected bool HasTriggered = false;
    public static List<EventBase> events = new List<EventBase>();
    public AudioClip soundClip;
    public AudioSource audioSource;

    //Called when an event wants to be triggered
    public virtual bool OnEventTrigger()
    {
        if (CanTrigger)
        {
            if (!MultipleTriggers)
                HasTriggered = true;
            else
                StartCoroutine(EventCoolDown());

            EventTrigger();
            CanTrigger = false;

            if (HasTriggered)
                Destroy(gameObject);

            return true;
        }
        return false;
    }

    //Coroutine event cooldown
    protected IEnumerator EventCoolDown()
    {
        yield return new WaitForSeconds(eventCooldown);

        CanTrigger = true;
    }

    //Actual code for executing event
    public abstract void EventTrigger();

    //Add event to the list of possible events
    void Awake()
    {
        events.Add(this);
        audioSource = GetComponent<AudioSource>();
    }

    //Remove event from the list of possible events
    void OnDisable()
    {
        events.Remove(this);
    }
}
