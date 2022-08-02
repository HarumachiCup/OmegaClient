using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ModGroup
{
    public Mod mod;
    public List<Map> maps;

    public ModGroup(Mod mod, List<Map> maps)
    {
        this.mod = mod;
        this.maps = maps;
    }
}

public class Map
{
    public int DiffID;
    public string MapID;
    public Map(int diff, string id = "?")
    {
        DiffID = diff;
        MapID = id;
    }
}

public static class Mappool
{
    public static List<ModGroup> MapPool;
    public static List<BeatmapPanel> Panels;
}

public class MappoolManager : MonoBehaviour
{
    [SerializeField]
    

    // 0 = dt
    // 1 = hd
    // 2 = hr
    // 3 = nm
    // 4 = fm
    // 5 = fl
    // 6 = tiebreaker

    

    public async void LoadFromSheet(string id, string sheet = null)
    {
        string url = "https://docs.google.com/spreadsheets/d/" + id + "/export?format=csv&id=" + id;
        if(sheet != null)
        {
            url += "&gid=" + sheet;
        }
        Debug.Log(url);
        string data = await APIConnector.SendRequest(url);
        string[] maps = data.Split('\n');
        Mappool.MapPool = new List<ModGroup>();
        foreach(string map in maps)
        {
            string[] split = map.Split(',');
            string modname = split[0];
            string name = split[1];
            string diff = split[2];
            int mod = Mods.GetModByName(modname);
            bool groupExists = false;
            Debug.Log("map from sheet: " + modname + " - " + name + " - " + diff);
            foreach(ModGroup group in Mappool.MapPool)
            {
                if(group.mod == Mods.ModList[mod])
                {
                    groupExists = true;
                    break;
                }
            }
            if(groupExists == false)
            {
                Mappool.MapPool.Add(new ModGroup(Mods.ModList[mod], new List<Map>()));
            }

            foreach (ModGroup group in Mappool.MapPool)
            {
                if (group.mod == Mods.ModList[mod])
                {
                    group.maps.Add(new Map(int.Parse(diff), name));
                    break;
                }
            }
        }
        UpdateUI();
    }

    public void TestPool()
    {
        Mappool.MapPool = new List<ModGroup>();
        UpdateUI();
    }

    void Start()
    {
        TestPool();
    }

    public GameObject RowPrefab;
    public GameObject RowContainer;
    public GameObject ModSectionPrefab;
    public GameObject BeatmapPanelMicro;

    void UpdateUI()
    {
        foreach (Transform child in RowContainer.transform)
        {
            Destroy(child.gameObject);
        }
        int counter = 0; // if this reaches 3 we need to go to next row and reset it
        GameObject currentRow = Instantiate(RowPrefab, RowContainer.transform);
        Mappool.Panels = new List<BeatmapPanel>();
        foreach (ModGroup group in Mappool.MapPool)
        {
            if ((counter + group.maps.Count() > 4 && Mappool.MapPool.Count() >= 8) || (counter + group.maps.Count() > 3 && Mappool.MapPool.Count() < 8))
            {
                // make new current row
                currentRow = Instantiate(RowPrefab, RowContainer.transform);
                counter = 0;
            }

            Mod thisMod = group.mod;
            GameObject modSection = Instantiate(ModSectionPrefab, currentRow.transform);

            modSection.GetComponent<Image>().color = thisMod.AccentColour;
            modSection.GetComponentInChildren<Text>().color = thisMod.TextColour;
            modSection.GetComponentInChildren<Text>().text = thisMod.Name.ToUpper();

            GameObject mapContainer = null;
            foreach (Transform child in modSection.transform)
            {
                if (child.name == "MapContainer")
                {
                    mapContainer = child.gameObject;
                }
            }
            foreach (Map x in group.maps)
            {
                GameObject beatmapPanel = Instantiate(BeatmapPanelMicro, mapContainer.transform);
                Mappool.Panels.Add(beatmapPanel.GetComponent<BeatmapPanel>());
                beatmapPanel.GetComponent<BeatmapPanel>().DifficultyID = x.DiffID;
                beatmapPanel.GetComponent<BeatmapPanel>().MappoolID = x.MapID;
                beatmapPanel.GetComponent<BeatmapPanel>().UpdatePanel();
                counter++;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
