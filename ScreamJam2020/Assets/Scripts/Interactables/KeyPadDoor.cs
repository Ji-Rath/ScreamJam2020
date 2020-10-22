using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class KeyPadDoor : Door
{
    public GameObject keypadUI;
    public string keypadCode;
    public AudioClip unlockSound;
    public AudioClip buttonSound;
    public int maxDigits;
    public TextMeshProUGUI codeText;
    [Tooltip("Message to display when the door is locked"), TextArea]
    public string lockedMessage;

    public string currentCode;
    private GameObject player;
    private bool isKeypadMenuOpen;
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();

        player = GameManager.Get().playerRef;
        keypadUI.SetActive(false);
    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Interact"))
        {
            if (isKeypadMenuOpen)
            {
                Debug.Log("lol");
                OpenCloseKeypad();
            }
        }
        
    }

    public void CheckCode()
    {
        if (currentCode == keypadCode)
        {
            isLocked = false;
            currentCode = "UNLOCKED";
            codeText.text = currentCode;
            if(audioSource)
            {
                audioSource.clip = unlockSound;
                audioSource.Play();
            }
        }
        else
        {
            currentCode = "";
            codeText.text = currentCode;
            DialogueBox.Get().TriggerText(lockedMessage);
            if (audioSource)
            {
                audioSource.clip = lockedSound;
                audioSource.Play();
            }
        }
    }

    public override void OnInteract()
    {
        if (isLocked)
        {
            OpenCloseKeypad();
        }
        else
        {
            base.OnInteract();
        }
    }

    public void AddCharacter(string character)
    {
        if (currentCode.Length < maxDigits)
        {
            currentCode += character;
            codeText.text = currentCode;

            if (audioSource)
            {
                audioSource.clip = buttonSound;
                audioSource.Play();
            }

        }
    }

    public void DeleteCharacter()
    {
        if(currentCode.Length > 0)
        {
            string temp = "";
            temp = currentCode.Substring(0, currentCode.Length - 1);
            currentCode = temp;
            codeText.text = currentCode;

            if (audioSource)
            {
                audioSource.clip = buttonSound;
                audioSource.Play();
            }
        }
    }

    public void OpenCloseKeypad()
    {
        keypadUI.SetActive(!keypadUI.activeSelf);

        player.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(!keypadUI.activeSelf);
        player.GetComponent<FirstPersonController>().enabled = !keypadUI.activeSelf;

        Invoke("SetKeyPadMenuOpen", 0.1f);
    }

    private void SetKeyPadMenuOpen()
    {
        isKeypadMenuOpen = keypadUI.activeSelf;
    }
}
