using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerManager_Mappool : MonoBehaviour
{
    public Button B_switchToRed;
    public Button B_switchToBlue;
    public string CurrentTeam;
    public InputField Ip_Map;
    public Button B_select;
    public Button b_deselect;

    public InputField Ip_SheetName;
    public InputField Ip_SheetID;
    public Button B_updateMappool;

    public MappoolManager mappoolManager;

    private void Start()
    {
        B_switchToRed.onClick.AddListener(delegate { SwitchActive("tored"); });
        B_switchToBlue.onClick.AddListener(delegate { SwitchActive("toblue"); });
        B_select.onClick.AddListener(Select);
        b_deselect.onClick.AddListener(Deselect);
        B_updateMappool.onClick.AddListener(delegate
        {
            mappoolManager.LoadFromSheet(Ip_SheetName.text, Ip_SheetID.text);
        });
    }


    public void SetActive(Button button, bool active)
    {
        if (active)
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 1f);
        }
        else
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 0.25f);
        }
    }

    public void SwitchActive(string teamOverride = null)
    {
        string team = CurrentTeam;
        if (teamOverride != null) team = teamOverride;
        if (team == "red" || team == "toblue")
        {
            CurrentTeam = "blue";
            SetActive(B_switchToBlue, true);
            SetActive(B_switchToRed, false);
        }
        else if (team == "blue" || team == "tored") 
        {
            CurrentTeam = "red";
            SetActive(B_switchToBlue, false);
            SetActive(B_switchToRed, true);
        }
    }

    public void UnsetActive()
    {
        SetActive(B_switchToBlue, false);
        SetActive(B_switchToRed, false);
    }

    void Select()
    {
        string mapName = Ip_Map.text;
        Ip_Map.text = "";
        BeatmapPanel xref = null;
        foreach (BeatmapPanel panel in Mappool.Panels)
        {
            if(panel.MappoolID.ToLower() == mapName.ToLower())
            {
                xref = panel;
                break;
            }
        }
        if(xref != null)
        {
            xref.Selected = true;
            xref.Disabled = false;
            xref.SelectionTeam = CurrentTeam;
            xref.UpdateUIState();
        }
        SwitchActive();
    }

    void Deselect()
    {
        string mapName = Ip_Map.text;
        Ip_Map.text = "";
        BeatmapPanel xref = null;
        foreach (BeatmapPanel panel in Mappool.Panels)
        {
            if (panel.MappoolID.ToLower() == mapName.ToLower())
            {
                xref = panel;
                break;
            }
        }
        if (xref != null)
        {
            xref.Selected = true;
            xref.Disabled = false;
            xref.SelectionTeam = CurrentTeam;
            xref.UpdateUIState();
        }
        UnsetActive();
    }
}
