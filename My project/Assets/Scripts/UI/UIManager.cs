using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] public Text scoreTxt, latScoreTxt, timerTxt;
    [SerializeField] Image hpBarImg;

    private void Start()
    {
        UpdateHpBarVisual(1);
        UpdateText(scoreTxt, "0");
        UpdateText(timerTxt, "0");
        UpdateText(latScoreTxt, "Last score: " + GameManager.Instance.LoadLastScore());
    }

    public void UpdateText(Text txt, string str)
    {
        txt.text = str;
    }

    public void UpdateHpBarVisual(float amount)
    {
        hpBarImg.fillAmount = amount;
    }
}
