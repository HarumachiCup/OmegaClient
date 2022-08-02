using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class APIConnector
{
    public static int bmRequestCounter;

    public class ApiError
    {
        public string error;
        public ApiError(string error)
        {
            this.error = error;
        }
    }

    public static async Task<string> SendRequest(string path, string type = "get", WWWForm form = null)
    {
        UnityWebRequest uwr;
        if (type == "get")
        {
            uwr = UnityWebRequest.Get(path);
        }
        else
        {
            if(form == null)
            {
                Debug.LogError("Form not passed to POST request. Consider a GET request instead.");
                return null;
            }

            uwr = UnityWebRequest.Post(path, form);
        }

        await uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.LogError("Error While Sending: " + uwr.error);
            return "ERROR:" + uwr.error;
        }
        else
        {
            return uwr.downloadHandler.text;
        }
    }

    public static async Task<Texture2D> GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        await request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            return ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        return null;
    }
}

public static class ExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
}