using System.Collections;
using UnityEngine;

public class Sharing : MonoBehaviour
{
    private string _screenshotPath;
    [SerializeField] private CompletedPanel _completedPanel;

    private void Start()
    {
        _screenshotPath = Application.persistentDataPath + "/screenshot.png";
    }

    public void Share()
    {
        StartCoroutine(ShareImg());
    }

    private IEnumerator ShareImg()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = GameUIManager.Instance.GetScreenshot();

        System.IO.File.WriteAllBytes(_screenshotPath, screenshot.EncodeToPNG());

        // Sử dụng callback của NativeShare
        new NativeShare()
            .AddFile(_screenshotPath)
            .SetSubject("This is my score")
            .SetText("Share to your friends")
            .SetCallback((result, shareTarget) =>
            {
                if (result == NativeShare.ShareResult.Shared)
                {
                    _completedPanel.ShareCompleted();
#if UNITY_EDITOR
                    Debug.Log("Share completed successfully");
#endif
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Share failed or unknown result");
#endif
                }
            })
            .Share();
    }
}