using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerManager : MonoBehaviour
{
    public UserInfoManager userInfoManager;
    public WinOverlayManagr winOverlayManager;

    public Button b_addPointToRed;
    public Button b_addPointToBlue;
    public Button b_removePointFromRed;
    public Button b_removePointFromBlue;
    public Button b_updateUserInfoRed;
    public Button b_updateUserInfoBlue;
    public Button b_declareWinnerRed;
    public Button b_declareWinnerBlue;
    public InputField ip_userIdBlue;
    public InputField ip_userIdRed;
    public InputField ip_commentators;
    public InputField ip_tech;
    public InputField ip_round;
    public Button b_resetPoints;
    public Button b_hideWinnerScreen;
    public Button b_harumachiClover;
    public Button b_generalUpdate;
    public InputField ip_ipcLocation;
    public AudioSource harumachiClover;
    public InputField ip_firstTo;
    public Button b_showPlayScreen;
    public Button b_showMappoolScreen;
    public Button b_reflowOsuWindows;
    public Toggle t_showUI;
    public GameObject uiPreview;
    public UITextsManager textsManager;

    void Start()
    {
        b_addPointToRed.onClick.AddListener(AddPointToRed);
        b_addPointToBlue.onClick.AddListener(AddPointToBlue);
        b_removePointFromRed.onClick.AddListener(RemovePointFromRed);
        b_removePointFromBlue.onClick.AddListener(RemovePointFromBlue);
        b_updateUserInfoRed.onClick.AddListener(UpdateUserInfoRed);
        b_updateUserInfoBlue.onClick.AddListener(UpdateUserInfoBlue);
        b_declareWinnerRed.onClick.AddListener(DeclareWinnerRed);
        b_declareWinnerBlue.onClick.AddListener(DeclareWinnerBlue);
        b_resetPoints.onClick.AddListener(ResetPoints);
        b_hideWinnerScreen.onClick.AddListener(HideWinnerScreen);
        b_harumachiClover.onClick.AddListener(HarumachiClover);
        b_generalUpdate.onClick.AddListener(GeneralUpdate);
        ip_firstTo.onValueChanged.AddListener(FirstToUpdate);
        b_showPlayScreen.onClick.AddListener(delegate { SwitchScreen(Global.Screens.Play); });
        b_showMappoolScreen.onClick.AddListener(delegate { SwitchScreen(Global.Screens.Mappool); });
        b_reflowOsuWindows.onClick.AddListener(ReflowOsuWindows);
        b_generalUpdate.onClick.AddListener(UpdateTexts);
        t_showUI.onValueChanged.AddListener(delegate
        {
            uiPreview.SetActive(t_showUI.isOn);
        });
    }
    public void UpdateTexts()
    {
        textsManager.CommentatorsText = ip_commentators.text;
        textsManager.TechText = ip_tech.text;
        textsManager.RoundText = ip_round.text;
        textsManager.UpdateTexts();
        Global.CurrentRound = textsManager.RoundText;
    }
    public void SwitchScreen(Global.Screens Screen)
    {
        Global.CurrentScreen = Screen;
        if(Screen == Global.Screens.Mappool)
        {
            foreach (BeatmapPanel panel in Mappool.Panels)
            {
                panel.Disabled = true;
                panel.UpdateUIState();
            }
        }
    }


    void HarumachiClover()
    {
        harumachiClover.Play();
    }

    void AddPointToRed()
    {
        if (userInfoManager.red.Points < userInfoManager.FirstTo)
        {
            userInfoManager.red.Points++;
        }
        userInfoManager.UpdateUI();
    }
    void AddPointToBlue()
    {
        if (userInfoManager.blue.Points < userInfoManager.FirstTo)
        {
            userInfoManager.blue.Points++;
        }
        userInfoManager.UpdateUI();
    }
    void RemovePointFromRed()
    {
        if (userInfoManager.red.Points > 0)
        {
            userInfoManager.red.Points--;
        }
        userInfoManager.UpdateUI();
    }
    void RemovePointFromBlue()
    {
        if (userInfoManager.blue.Points > 0)
        {
            userInfoManager.blue.Points--;
        }
        userInfoManager.UpdateUI();
    }
    void UpdateUserInfoBlue()
    {
        userInfoManager.blue.id = int.Parse(ip_userIdBlue.text);
        userInfoManager.UpdatePlayer("blue");
    }
    void UpdateUserInfoRed()
    {
        userInfoManager.red.id = int.Parse(ip_userIdRed.text);
        userInfoManager.UpdatePlayer("red");
    }
    void DeclareWinnerBlue()
    {
        winOverlayManager.ShowWinnerScreen("blue");
    }
    void DeclareWinnerRed()
    {
        winOverlayManager.ShowWinnerScreen("red");
    }
    void GeneralUpdate()
    {
        // TODO
    }
    void ResetPoints()
    {
        userInfoManager.red.Points = 0;
        userInfoManager.blue.Points = 0;
        userInfoManager.UpdateUI();
    }
    void HideWinnerScreen()
    {
        winOverlayManager.HideWinnerScreen();
    }
    public void FirstToUpdate(string arg0)
    {
        userInfoManager.FirstTo = int.Parse(ip_firstTo.text);
        userInfoManager.UpdateUI();
    }
    public void ReflowOsuWindows()
    {
        Resizer.Resize();
    }
}

public class Resizer
{
    public static void Resize()
    {
        Process.Start(@"K:\Harumachi Cup Omega Client\WindowMover\bin\Debug\ResizeTourneyClientWindows.exe");
        // code used to be here, but didn't work, so now it's a seperate exe
    }
}
