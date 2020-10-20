using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathScreen : MonoBehaviour
{
    [Header("Hands Config"), Space]
    public Image handsLeft;
    public Image handsRight;
    public float alphaMaxValue;
    public float alphaValueSpeed;
    public float fillAmountMultiplier;
    public float minDistanceToAppear;

    private Color newColor;
    private float alphaValue;
    private float finalFillAmount;
    public GameObject enemy;
    private GameObject player;
    private bool doOnce;
    private bool appear;
    private bool disAppear;

    [Header("Death Screen Config"), Space]
    public CanvasGroup canvas;
    public GameObject deathPanel;
    public float newAlphaValueSpeed;
    private float newAlphaValue;
    private Color deathScreenColor;
    public bool isPlayerDead;
    private bool deathScreenFinish = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Get().playerRef;
        handsLeft.fillAmount = 0;
        handsRight.fillAmount = 0;
        newColor = handsLeft.color;
        //deathScreenColor = deathScreen.color;
        if(deathPanel)
        {
            canvas.gameObject.SetActive(false);
            deathPanel.SetActive(false);
        }
        appear = true;

        MonsterAI.OnMonsterKillPlayer += StartDeathScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayerDead)
        {
            UpdateHands();
        }
        else
        {
            UpdateDeathScreen();
        }
        
    }

    public void UpdateHands()
    {
        if (enemy.activeSelf)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

            if (distance <= minDistanceToAppear)
            {
                if (appear)
                {
                    alphaValue += Time.deltaTime * alphaValueSpeed;

                    if (alphaValue >= alphaMaxValue)
                    {
                        alphaValue = alphaMaxValue;
                        appear = false;
                    }
                }


                doOnce = false;
                finalFillAmount = fillAmountMultiplier / distance;
                if (finalFillAmount >= 0.5f)
                {
                    finalFillAmount = 0.5f;
                }
                handsLeft.fillAmount = finalFillAmount;
                handsRight.fillAmount = finalFillAmount;
                newColor.a = alphaValue;
                handsLeft.color = newColor;
                handsRight.color = newColor;
            }
            else
            {
                UpdateClearScreen();
            }
        }
        else
        {
            UpdateClearScreen();
        }
    }

    public void StartDeathScreen()
    {
        isPlayerDead = true;
    }

    public void UpdateClearScreen()
    {
        if (!doOnce)
        {
            alphaValue = alphaMaxValue;
            doOnce = true;
            appear = true;
            disAppear = true;
        }

        if (disAppear)
        {
            alphaValue -= Time.deltaTime * alphaValueSpeed;

            if (alphaValue <= 0)
            {
                alphaValue = 0;
                handsLeft.fillAmount = 0;
                handsRight.fillAmount = 0;
                disAppear = false;
                //appear = true;
            }

            newColor.a = alphaValue;
            handsLeft.color = newColor;
            handsRight.color = newColor;
        }
    }

    public void UpdateDeathScreen()
    {
        if(!deathScreenFinish)
        {
            canvas.gameObject.SetActive(true);
            newAlphaValue += Time.deltaTime * newAlphaValueSpeed;
            if (newAlphaValue >= 1)
            {
                player.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(false);
                player.GetComponent<FirstPersonController>().enabled = false;
                newAlphaValue = 1;
                deathScreenFinish = true;
                deathPanel.SetActive(true);
            }

            canvas.alpha = newAlphaValue;
        }
        
    }

    private void OnDestroy()
    {
        MonsterAI.OnMonsterKillPlayer -= StartDeathScreen;
    }
}
