using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToggleLightFlickerTimed : EventTimedBase
{
    [Header("Toggle Config"), Space]
    public List<FlickeringEffect> toggleComponent = new List<FlickeringEffect>();
    [Tooltip("Whether to make component disabled or enabled for a set time")]
    public bool SetEnable;
    

    public override IEnumerator TimedEvent()
    {
        //Set components to specified state
        foreach (FlickeringEffect component in toggleComponent)
            component.activateFlicker = !SetEnable;

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
        foreach (FlickeringEffect component in toggleComponent)
            component.activateFlicker = SetEnable;
    }
}
