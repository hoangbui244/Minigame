using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private static GameObject m_canvas;

    private GameObject m_overlay;

    private void Awake()
    {
        m_canvas = new GameObject("TransitionCanvas");
        var canvas = m_canvas.AddComponent<Canvas>();
        canvas.sortingOrder = 10;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        DontDestroyOnLoad(m_canvas);
    }

    public static void LoadLevel(string level, float duration, Color color)
    {
        var fade = new GameObject("Transition");
        fade.AddComponent<Transition>();
        fade.GetComponent<Transition>().StartFade(level, duration, color);
        fade.transform.SetParent(m_canvas.transform, false);
        fade.transform.SetAsLastSibling();
    }

    private void StartFade(string level, float duration, Color fadeColor)
    {
        StartCoroutine(RunFade(level, duration, fadeColor));
    }

    private IEnumerator RunFade(string level, float duration, Color fadeColor)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, fadeColor);
        bgTex.Apply();

        m_overlay = new GameObject();
        var image = m_overlay.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;
        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);

        m_overlay.transform.localScale = new Vector3(1, 1, 1);
        m_overlay.GetComponent<RectTransform>().sizeDelta = m_canvas.GetComponent<RectTransform>().sizeDelta;
        m_overlay.transform.SetParent(m_canvas.transform, false);
        m_overlay.transform.SetAsFirstSibling();

        var time = 0.0f;
        var halfDuration = duration / 2;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            yield return null;
        }

        image.canvasRenderer.SetAlpha(1.0f);
        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene(level);

        time = 0.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
            yield return null;
        }

        image.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForEndOfFrame();

        Destroy(m_canvas);
    }
}