using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class IPCManager : MonoBehaviour
{
    public UserInfoManager userInfoManager;
    public struct chatMessage
    {
        public string author;
        public string message;
    }
    public string[] scores;
    public string state;
    public string path = @"K:\Harumachi Cup Omega Client\stableclient";
    public string[] chat;
    public Text t_Chat;
    float time = 0;
    public List<chatMessage> messages;

    public int mapId;
    public BeatmapPanel mainBeatmapPanel;

    public void processChat()
    {
        messages = new List<chatMessage>();
        foreach(string x in chat)
        {
            string username = x.Split(new char[] { ':' }, 2)[0];
            string message = x.Split(new char[] { ':' }, 2)[1].Remove(0, 1);
            chatMessage msg = new chatMessage();
            msg.author = username;
            msg.message = message;
            messages.Add(msg);
        }
    }

    void updateBeatmapPanel()
    {
        Debug.Log("Updating Beatmap");
        mainBeatmapPanel.DifficultyID = mapId;
        int[] mod = new int[] { 3 };
        foreach (ModGroup group in Mappool.MapPool)
        {
            foreach(Map map in group.maps)
            {
                if(map.DiffID == mapId)
                {
                    Debug.Log("Found map! - " + mapId);
                    Debug.Log("mod is " + Mods.GetModByName(group.mod.Acronym));
                    mod = new int[] { Mods.GetModByName(group.mod.Acronym) };
                    break;
                } else
                {
                    Debug.Log("map is not " + map.DiffID + " - " + mapId);
                }
            }
        }
        mainBeatmapPanel.UIMods = mod;
        mainBeatmapPanel.UpdatePanel();
    }

    void Update()
    {
        if (time > 0.5f)
        {
            time = 0;

            chat = File.ReadAllLines(path + @"\" + "chat.log");
            processChat();
            t_Chat.text = "";
            foreach (chatMessage msg in messages)
            {
                string colour;
                if (msg.author == userInfoManager.red.Username)
                {
                    colour = "FF5F85";
                }
                else if (msg.author == userInfoManager.blue.Username)
                {
                    colour = "5FC5FF";
                }
                else
                {
                    colour = "F6FA4D";
                }
                string output;
                string message = msg.message.Replace("</b>", "");
                output = "<color=#" + colour + ">" + msg.author + "</color>: <b>" + message + "</b>";
                t_Chat.text += "\n" + output;
            }

            state = File.ReadAllText(path + @"\" + "ipc-state.txt");

            int tmpMapId = int.Parse(File.ReadAllLines(@"C:\Program Files (x86)\StreamCompanion\Files\mapid.txt")[0]);
            if(mapId != tmpMapId)
            {
                mapId = tmpMapId;
                updateBeatmapPanel();
            }
        }
        if (state == "Playing")
        {
            scores = File.ReadAllLines(path + @"\" + "ipc-scores.txt");
            IPC.redScore = scores[0];
            IPC.blueScore = scores[1];
        }

        
        IPC.state = state;
        time += Time.deltaTime;
    }
}

public static class IPC
{
    public static string redScore;
    public static string blueScore;
    public static string state;
} 