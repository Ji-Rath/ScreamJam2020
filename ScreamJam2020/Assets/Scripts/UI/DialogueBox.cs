using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonobehaviourSingleton<DialogueBox>
{
    public TextMeshProUGUI text;
    public float textSpeed;
    public float textTime;
    public float textTimer;
    public float appearTime;
    public float disappearTime;
    private Color alphaColor;
    private bool isEnabled;
    private bool canAppear;
    private bool canDisappear;
    // Start is called before the first frame update
    void Start()
    {
        alphaColor = text.color;
        alphaColor.a = 0;
        text.color = alphaColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnabled)
        {
            if(canAppear)
            {
                alphaColor.a += Time.deltaTime * textSpeed;
                if (alphaColor.a >= 1)
                {
                    canAppear = false;
                    alphaColor.a = 1;
                }
                text.color = alphaColor;
            }

            if(!canAppear && !canDisappear)
            {
                textTimer += Time.deltaTime;

                if(textTimer >= textTime)
                {
                    textTimer = 0;
                    canDisappear = true;
                }
            }

            if (canDisappear)
            {
                alphaColor.a -= Time.deltaTime * textSpeed;
                if (alphaColor.a <= 0)
                {
                    canDisappear = false;
                    isEnabled = false;
                    alphaColor.a = 0;
                }
                text.color = alphaColor;
            }

        }
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void TriggerText(float newTextTime)
    {
        alphaColor.a = 0;
        text.color = alphaColor;
        isEnabled = true;
        canAppear = true;
        textTime = newTextTime;
    }
}
