using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Global
{
    public static string ApiKey = "YOUR_API_KEY_HERE"; // https://osu.ppy.sh/p/api
    public enum Screens
    {
        Play,
        Mappool
    }
    public static Screens CurrentScreen;
    public static string CurrentRound;
}