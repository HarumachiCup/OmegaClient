using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorebar : MonoBehaviour
{
    public float RawRedPoints;
    public float RawBluePoints;
    public float SmoothedRedPoints;
    public float SmoothedBluePoints;
    public float velocity1;
    public float velocity2;
    public float BlueBarWidth;
    public float RedBarWidth;
    public RectTransform RedBar;
    public RectTransform BlueBar;
    public Text RedScore;
    public Text BlueScore;
    public string RedScoreText;
    public string BlueScoreText;
    public float Base;
    public float Multiplier;
    public float Pow;
    public float Div;
    bool winning = false; // false = red, true = blue
    // IMPORTANT
    // when setting the width of the score bars, due to padding and the sprites having padding for glow,
    // you need to add 56px. at 56px width it displays as 0px, so have to account for it!!!

    void Update()
    {
        if(IPC.redScore == null)
        {
            IPC.redScore = "0";
        }
        if (IPC.blueScore == null)
        {
            IPC.blueScore = "0";
        }

        RawRedPoints = float.Parse(IPC.redScore);
        RawBluePoints = float.Parse(IPC.blueScore);
        SmoothedRedPoints = Mathf.SmoothDamp(SmoothedRedPoints, RawRedPoints, ref velocity1, 0.8f);
        SmoothedBluePoints = Mathf.SmoothDamp(SmoothedBluePoints, RawBluePoints, ref velocity2, 0.8f);
        RedScoreText = Mathf.RoundToInt(SmoothedRedPoints).ToString();
        BlueScoreText = Mathf.RoundToInt(SmoothedBluePoints).ToString();
        if (SmoothedRedPoints > SmoothedBluePoints)
        {
            // red is winning
            RedBarWidth = (56 + (Mathf.Pow(Mathf.Log(((SmoothedRedPoints - SmoothedBluePoints)/Div), Base), Pow) * Multiplier));
            BlueBarWidth = 56;
            RedScore.fontSize = 40;
            BlueScore.fontSize = 30;
            RedScoreText = "<b>" + RedScoreText + "</b>";
        }
        else if(SmoothedBluePoints > SmoothedRedPoints)
        {
            // blue is winning
            BlueBarWidth = (56 + (Mathf.Pow(Mathf.Log(((SmoothedBluePoints - SmoothedRedPoints)/Div), Base), Pow) * Multiplier));
            RedBarWidth = 56;
            RedScore.fontSize = 30;
            BlueScore.fontSize = 40;
            BlueScoreText = "<b>" + BlueScoreText + "</b>";
        }
        RedBar.sizeDelta = new Vector2(RedBarWidth, RedBar.sizeDelta.y);
        BlueBar.sizeDelta = new Vector2(BlueBarWidth, BlueBar.sizeDelta.y);
        RedScore.text = RedScoreText;
        BlueScore.text = BlueScoreText;
    }
}
