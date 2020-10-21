using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Lantern : MonoBehaviour
{
    public bool isLanternOn;
    public GameObject lanternObject;
    public GameObject lanternLight;
    private Animator lanternAnimator;
    private AudioSource audioSource;
    public AudioClip lanternONSound;
    public AudioClip lanternOFFSound;
    public bool canSwitchLantern = true;


    // Start is called before the first frame update
    void Start()
    {
        lanternAnimator = lanternObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Lantern"))
        {
            if (canSwitchLantern)
            {
                SwitchOnOffLantern();
            }
        }
    }

    public void CanSwitch()
    {
        canSwitchLantern = true;
    }

    public void CannotSwitch()
    {
        canSwitchLantern = false;
    }

    private void SwitchOnOffLantern()
    {
        if (canSwitchLantern)
        {
            isLanternOn = !isLanternOn;
            if (isLanternOn)
            {
                lanternAnimator.SetTrigger("On");
                lanternLight.SetActive(true);
                if (audioSource)
                {
                    audioSource.clip = lanternONSound;
                    audioSource.Play();
                }
                
            }
            else
            {
                lanternAnimator.SetTrigger("Off");
                lanternLight.SetActive(false);
                if (audioSource)
                {
                    audioSource.clip = lanternOFFSound;
                    audioSource.Play();
                }
                
            }
        }
    }
}
