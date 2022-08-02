using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PresetManager : MonoBehaviour
{
    public GameObject button;
    public GameObject buttonContainer;
    [SerializeField]
    public List<Preset> presets;
    public string directory = @"K:\Harumachi Cup Omega Client\Presets\";

    public UIControllerManager uicm;
    public UIControllerManager_Mappool uicm_mp;

    void Start()
    {
        presets = new List<Preset>();

        DirectoryInfo d = new DirectoryInfo(directory);
        Debug.Log(d);
        foreach (var file in d.GetFiles("*"))
        {
            Debug.Log(file.FullName);
            string jsonString = File.ReadAllText(file.FullName);
            Preset data = JsonUtility.FromJson<Preset>(jsonString);
            presets.Add(data);
            GameObject nb = Instantiate(button, buttonContainer.transform);
            nb.GetComponentInChildren<Text>().text = data.preset_name;
            nb.GetComponent<Button>().onClick.AddListener(delegate
            {
                SetActive(data);
            });
        }
    }

    public void SetActive(Preset preset)
    {
        uicm.ip_commentators.text = preset.commentators;
        uicm.ip_round.text = preset.round_name;
        uicm.ip_firstTo.text = preset.first_to.ToString();

        uicm_mp.Ip_SheetID.text = preset.sheet_id.ToString();

        uicm.UpdateTexts();
        uicm.FirstToUpdate(uicm.ip_firstTo.text);

        uicm_mp.mappoolManager.LoadFromSheet(uicm_mp.Ip_SheetName.text, uicm_mp.Ip_SheetID.text);
    }
}

[System.Serializable]
public struct Preset
{
    public string preset_name;
    public int first_to;
    public int sheet_id;
    public string commentators;
    public string round_name;
}