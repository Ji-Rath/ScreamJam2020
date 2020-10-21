using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToggleComponent : EventBase
{
    [Header("Toggle Config"), Space]
    public List<Behaviour> toggleComponent = new List<Behaviour>();
    [Tooltip("Whether to make component disabled or enabled")]
    public bool SetDisabled;
    

    public override void EventTrigger()
    {
        foreach (Behaviour component in toggleComponent)
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
        
    }
}
