using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExplosion : EventBase
{
    [Header("Explosion Config"), Space]
    public float explosionForce = 100f;
    public float explosionRadius = 3f;
    public LayerMask layerMask;
    public override void EventTrigger()
    {
        Collider[] physicsObjects = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        //Loop through all nearby objects
        foreach (Collider physicsObject in physicsObjects)
        {
            Rigidbody rigidbody = physicsObject.GetComponent<Rigidbody>();

            if(rigidbody)
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, 5f, ForceMode.Impulse);
        }

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
