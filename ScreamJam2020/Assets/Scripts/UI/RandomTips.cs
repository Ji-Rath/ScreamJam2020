using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTips : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string titleText;
    public string[] tips;

    public void SetRandomTip()
    {
        if(tips.Length > 0)
        {
            int randomIndex = Random.Range(0, tips.Length);
            text.text = titleText + tips[randomIndex];
        }
    }
}
