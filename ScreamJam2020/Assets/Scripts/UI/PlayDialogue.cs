using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    [System.Serializable]
    public class TextProperties
    {
        public string elementTitle;
        public string text;
        public float textTime;
    }

    public bool activateWithTrigger;
    public bool triggerOnce;
    public TextProperties[] allText;

    private bool startDialogue;
    private int dialogueIndex;
    private DialogueBox dialogue;
    private float dialogueTimer;
    private bool doOnce;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueBox.Get();       
    }

    // Update is called once per frame
    void Update()
    {
        if(startDialogue)
        {
            if (!doOnce)
            {
                dialogue.SetText(allText[dialogueIndex].text);
                dialogue.TriggerText(allText[dialogueIndex].textTime);
                doOnce = true;
            }

            dialogueTimer += Time.deltaTime;

            if(dialogueTimer >= (allText[dialogueIndex].textTime + (dialogue.textSpeed / 2)))
            {
                doOnce = false;
                dialogueIndex++;
                dialogueTimer = 0;
                if (dialogueIndex >= allText.Length)
                {
                    dialogueIndex = 0;
                    startDialogue = false;
                    if(triggerOnce)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void Play()
    {
        startDialogue = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(activateWithTrigger)
            {
                Play();
            }
        }
    }
}
