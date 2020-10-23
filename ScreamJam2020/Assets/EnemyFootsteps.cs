using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyFootsteps : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> footsteps;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Step()
    {
        audioSource.clip = footsteps[Random.Range(0, footsteps.Count)];
        audioSource.Play();
    }
}
