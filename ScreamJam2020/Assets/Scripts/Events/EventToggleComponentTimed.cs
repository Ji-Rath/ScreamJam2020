using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToggleComponentTimed : EventTimedBase
{
    [Header("Toggle Config"), Space]
    public List<Behaviour> toggleComponent = new List<Behaviour>();
    [Tooltip("Whether to make component disabled or enabled for a set time")]
    public bool SetDisabled;

    public override IEnumerator TimedEvent()
    {
        //Set components to specified state
        foreach(Behaviour component in toggleComponent)
            component.enabled = !SetDisabled;

        if (audioSource)
        {
            if (soundClip)
            {
                GameObject audioObject = new GameObject();
                audioObject.name = "Audio Object";
                AudioSource source = audioObject.AddComponent<AudioSource>();
                source.clip = soundClip;
                source.Play();
                Destroy(audioObject, soundClip.length);
            }
        }

        if (dialogueText != "")
        {
            DialogueBox.Get().SetText(dialogueText);
            DialogueBox.Get().TriggerText(dialogueTime);
        }

        yield return new WaitForSeconds(eventTime);

        //Set components back to default state
        foreach (Behaviour component in toggleComponent)
            component.enabled = SetDisabled;
    }
}
