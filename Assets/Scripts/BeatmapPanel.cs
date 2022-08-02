using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Mod
{
    public string Name;
    public string Acronym;
    public Color32 AccentColour;
    public Color32 TextColour;

    public Mod(string name, string acronym, Color32 accent, Color32 text)
    {
        Name = name;
        Acronym = acronym;
        AccentColour = accent;
        TextColour = text;
    }
}
[Serializable]
public static class Mods
{
    public static int GetModByName(string name)
    {
        name = name.ToLower();
        if (name == "dt")
        {
            return 0;
        }
        if (name == "hd")
        {
            return 1;
        }
        if (name == "hr")
        {
            return 2;
        }
        if (name == "nm")
        {
            return 3;
        }
        if (name == "fm")
        {
            return 4;
        }
        if (name == "fl")
        {
            return 5;
        }
        if (name == "tb")
        {
            return 6;
        }
        return 3; // return nomod in case of no mod. just a fallback
    }
    [SerializeField]
    public static Mod[] ModList = new Mod[]
    {
        new Mod("Double Time", "DT", new Color32(203,101,228, 255), new Color32(255,255,255,255)),
        new Mod("Hidden", "HD", new Color32(228, 79,31, 255), new Color32(255, 255, 255, 255)),
        new Mod("Hard Rock", "HR", new Color32(255, 1, 37, 255), new Color32(255,255,255,255)),
        new Mod("No Mod", "NM", new Color32(200, 200, 200, 255), new Color32(50, 50, 50, 255)),
        new Mod("Free Mod", "FM", new Color32(101, 228, 152, 255), new Color32(255, 255, 255, 255)),
        new Mod("Flashlight", "FL", new Color32(100, 32, 65, 255), new Color32(255, 255, 255, 255)),
        new Mod("Tiebreaker", "TB", new Color32(31, 157, 228, 255), new Color32(255, 255, 255, 255)),
    };
}

public class BeatmapPanel : MonoBehaviour
{
    public GameObject ModsContainer;
    public GameObject ModPrefab;
    public Text Title;
    public Text LowerTitle;
    public Text StarRating;
    public Text BPM;
    public Text CircleSize;
    public Text ApproachRate;
    public int DifficultyID;
    public string MappoolID;
    int BeatmapSetID;
    public RawImage Backdrop;
    public int[] UIMods = new int[] { 0, 1, 2, 3, 4 };
    public Text Micro_Artist;
    public bool Micro;

    public bool Selected = false;
    public bool Disabled = false;
    public string SelectionTeam;
    public Color TeamColour;
    public Image SelectionOverlay;
    public Sprite SelectionOverlaySprite;
    public Sprite SelectionOverlaySpriteDisabled;
    public CanvasGroup cg;
    public void UpdateUIState()
    {
        if (cg == null)
        {
            cg = this.gameObject.AddComponent<CanvasGroup>();
        }
        if (SelectionTeam == "red")
        {
            TeamColour = new Color32(217, 25, 37, 255);
        }
        else
        {
            TeamColour = new Color32(25, 211, 252, 255);
        }
        if(Selected == false)
        { try
            {
                SelectionOverlay.color = new Color(0, 0, 0, 0);
            }
            catch
            {

            }
        }
        else
        {
            SelectionOverlay.color = TeamColour;
            SelectionOverlay.sprite = SelectionOverlaySprite;
            cg.alpha = 1;
            if (Disabled)
            {
                SelectionOverlay.sprite = SelectionOverlaySpriteDisabled;
                cg.alpha = 0.5f;
            }
        }
    }

    public void UpdateModsUI()
    {
        if (Micro == false)
        {
            foreach (Transform child in ModsContainer.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (int mod in UIMods)
            {
                Mod refMod = Mods.ModList[mod];
                GameObject modObject = Instantiate(ModPrefab, ModsContainer.transform);
                modObject.GetComponentInChildren<Text>().color = refMod.TextColour;
                modObject.GetComponentInChildren<Text>().text = refMod.Acronym.ToUpper();
                modObject.GetComponent<Image>().color = refMod.AccentColour;

            }
        }
    }

    // 0 = dt
    // 1 = hd
    // 2 = hr
    // 3 = nm
    // 4 = fm
    // 5 = fl

    void Start()
    {
        //UpdatePanel();
        UpdateUIState();
    }

    public async void UpdatePanel()
    {
        while(APIConnector.bmRequestCounter > 2)
        {
            // we can't spam the osu! api too much. make sure we don't.
            // it'll start rate limiting us if we do
            await Task.Delay(1000);
        }
        APIConnector.bmRequestCounter++;
        string DiffInfo = await APIConnector.SendRequest("https://osu.ppy.sh/api/get_beatmaps?k=" + Global.ApiKey + "&b=" + DifficultyID);
        APIConnector.bmRequestCounter--;
        //Debug.Log(DiffInfo);
        dynamic json = JsonConvert.DeserializeObject(DiffInfo);
        try
        {
            json = json[0];
        }
        catch
        {
            Debug.LogError("Could not parse JSON from beatmap response. Likely rate limited. Jumping out.");
            UpdatePanel();
            return;
        }
        BeatmapSetID = json["beatmapset_id"];
        if (Micro == false)
        {
            Title.text = "<b>" + json["title"] + "</b><color=#fffa> - " + json["artist"] + "</color>";
        }
        else
        {
            if (Title.text != null)
            {
                try
                {
                    Title.text = "[" + MappoolID + "] <b>" + json["title"] + "</b>";
                }
                catch
                {

                }
            }
        }
        try
        {
            LowerTitle.text = "[" + json["version"] + "]<color=#ffffff88> - mapped by <b><color=#fff>" + json["creator"] + "</color></b></color>";
        }
        catch
        {
            return;
        }
        if (Micro == false) { 
            float starRating = float.Parse((string)json["difficultyrating"]);
            StarRating.text = (starRating).ToString("#.##");
            BPM.text = json["bpm"];
            CircleSize.text = json["diff_size"];
            ApproachRate.text = json["diff_approach"];
        }
        Texture2D BeatmapCover = await APIConnector.GetImage("https://assets.ppy.sh/beatmaps/" + BeatmapSetID + "/covers/cover.jpg");
        Backdrop.texture = BeatmapCover;
        UpdateModsUI();
        if(Micro)
        {
            Micro_Artist.text = json["artist"];
        }
        UpdateUIState();
    }
}