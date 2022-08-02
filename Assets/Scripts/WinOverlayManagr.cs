using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinOverlayManagr : MonoBehaviour
{
    public string Winner = "no";
    public Image InnerRingHorizontal;
    public Image InnerRingVertical;
    public Image OuterRing;
    public Image MiddleGlow;
    public Sprite InnerRingHorizontal_Blue;
    public Sprite InnerRingVertical_Blue;
    public Sprite OuterRing_Blue;
    public Sprite MiddleGlow_Blue;
    public Sprite InnerRingHorizontal_Red;
    public Sprite InnerRingVertical_Red;
    public Sprite OuterRing_red;
    public Sprite MiddleGlow_red;
    public Image PFPGlow;
    public Image PFPOverlay;
    public bool Shown = false;
    public UserInfoManager userInfoManager;

    public RawImage PFP;
    public Text Username;

    public Text RoundText;
    public Text VsText;

    public void ShowWinnerScreen(string winner = null)
    {
        RoundText.text = Global.CurrentRound.ToUpper();
        VsText.text = "<b>" + userInfoManager.red.Username + "</b> vs <b>" + userInfoManager.blue.Username + "</b>";

        Shown = true;
        if (winner == null) winner = Winner;
        else Winner = winner;

        if (Winner == "blue")
        {
            InnerRingHorizontal.sprite = InnerRingHorizontal_Blue;
            InnerRingVertical.sprite = InnerRingVertical_Blue;
            OuterRing.sprite = OuterRing_Blue;
            MiddleGlow.sprite = MiddleGlow_Blue;
            PFPGlow.color = new Color32(0, 168, 255, 255);
            PFPOverlay.color = new Color32(0, 168, 255, 255);
            PFP.texture = userInfoManager.blue.PFP;
            Username.text = userInfoManager.blue.Username;
        }
        else if (Winner == "red")
        {
            InnerRingHorizontal.sprite = InnerRingHorizontal_Red;
            InnerRingVertical.sprite = InnerRingVertical_Red;
            OuterRing.sprite = OuterRing_red;
            MiddleGlow.sprite = MiddleGlow_red;
            PFPGlow.color = new Color32(255, 0, 0, 255);
            PFPOverlay.color = new Color32(255, 0, 0, 255);
            PFP.texture = userInfoManager.red.PFP;
            Username.text = userInfoManager.red.Username;
        }

        // mess, but works
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<Animator>().StopPlayback();
        this.GetComponent<Animator>().Play("winscreen", 0, 0);
    }

    public void HideWinnerScreen()
    {
        this.GetComponent<Animator>().enabled = false;
        Shown = false;
    }
 

    CanvasGroup group;
    private void Update()
    {
        if (!Shown)
        {
            if(group.alpha > 0)
            {
                group.alpha -= (Time.deltaTime * 0.5f);
            }
        }
    }
    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }
}
