using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoManager : MonoBehaviour
{
    [Serializable]
    public struct Player
    {
        public int id;
        public string Username;
        public Texture2D PFP;
        public int Points;
        public Text Text;
        public RawImage Image;
        public GameObject PointContainer;
    }

    public int FirstTo = 5;
    [SerializeField]
    public Player red;
    [SerializeField]
    public Player blue;
    public GameObject PointActive;
    public GameObject PointInactive;

    public void UpdatePointCounter(Player player)
    {
        foreach(Transform child in player.PointContainer.transform)
        {
            Destroy(child.gameObject);
        }
        int activePoints = player.Points;
        if (activePoints > FirstTo) activePoints = FirstTo;
        int inactivePoints = FirstTo - player.Points;
        if (inactivePoints < 0) inactivePoints = 0;
        for (int i = 0; i < activePoints; i++)
        {
            Instantiate(PointActive, player.PointContainer.transform);
        }
        for (int i = 0; i < inactivePoints; i++)
        {
            Instantiate(PointInactive, player.PointContainer.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //red = new Player();
        //blue = new Player();
        red.Username = "Unset";
        blue.Username = "Unset";

        var texture = new Texture2D(64, 64);
        Color[] pixels = Enumerable.Repeat(new Color(255, 255, 0), 64 * 64).ToArray();
        texture.SetPixels(pixels);
        texture.Apply();

        red.PFP = texture;
        blue.PFP = texture;
        red.Points = 0;
        blue.Points = 0;

        blue.id = 2;
        UpdatePlayer("blue");
    }

    public async void UpdatePlayer(string side)
    {
        if(side == "all")
        {
            UpdatePlayer("red");
            UpdatePlayer("blue");
            return;
        }
        Player player;
        if (side == "red")
        {
            player = red;
        }
        else if (side == "blue")
        {
            player = blue;
        }
        else
        {
            Debug.LogError("Side is not blue or red.");
            return;
        }

        string PlayerInfo = await APIConnector.SendRequest("https://osu.ppy.sh/api/get_user?k=" + Global.ApiKey + "&u=" + player.id);
        Debug.Log(PlayerInfo);
        dynamic json = JsonConvert.DeserializeObject(PlayerInfo);
        try
        {
            player.Username = json[0]["username"];
        }catch
        {
            player.Username = "Could not fetch username";
        }
            // TODO: parse json to get username
            Texture2D PlayerPFP = await APIConnector.GetImage("https://a.ppy.sh/" + player.id);
        player.PFP = PlayerPFP;
        Debug.Log(PlayerInfo);

        if (side == "red")
        {
            red = player;
        }
        else if (side == "blue")
        {
            blue = player;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        red.Text.text = red.Username;
        red.Image.texture = red.PFP;
        blue.Text.text = blue.Username;
        blue.Image.texture = blue.PFP;
        UpdatePointCounter(red);
        UpdatePointCounter(blue);
        // todo: update point counter
    }
}
