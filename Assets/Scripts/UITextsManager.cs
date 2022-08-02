using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextsManager : MonoBehaviour
{
    public Text Bottom;
    public Text Round;
    public string CommentatorsText;
    public string TechText;
    public string RoundText;

    public void UpdateTexts()
    {
        Bottom.text = "commentators <b>" + CommentatorsText + "</b>\nstream & development <b>" + TechText + "</b>";
        Round.text = RoundText;
    }
}
