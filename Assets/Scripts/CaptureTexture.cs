using UnityEngine;

public class CaptureTexture : MonoBehaviour
{
    public int width = 320;
    public int height = 240;
    public MeshRenderer outputRenderer;
    Texture2D activeTex;
    public RenderTexture renderTexture;
    UnityCapture.Interface captureInterface;

    void Start()
    {
        captureInterface = new UnityCapture.Interface(UnityCapture.ECaptureDevice.CaptureDevice1);
        if (outputRenderer != null) outputRenderer.material.mainTexture = activeTex;
    }

    void OnDestroy()
    {
        captureInterface.Close();
    }

    void Update()
    {
        if (renderTexture != null)
        {
            UnityCapture.ECaptureSendResult result = captureInterface.SendTexture(renderTexture);
            if (result != UnityCapture.ECaptureSendResult.SUCCESS && result != UnityCapture.ECaptureSendResult.WARNING_CAPTUREINACTIVE)
                Debug.Log("SendTexture failed: " + result);
        }
    }
}
