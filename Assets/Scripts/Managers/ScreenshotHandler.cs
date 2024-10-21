using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    private Camera _cam;
    public RawImage ScreenshotDisplay;
    private Texture2D _screenshotTexture;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void CaptureScreenshot()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _cam.targetTexture = renderTexture;
        _cam.Render();

        RenderTexture.active = renderTexture;
        _screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenshotTexture.Apply();

        _cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
    }

    public void ShowScreenshot()
    {
        if (_screenshotTexture != null)
        {
            ScreenshotDisplay.texture = _screenshotTexture;
            ScreenshotDisplay.gameObject.SetActive(true);
        }
    }
}