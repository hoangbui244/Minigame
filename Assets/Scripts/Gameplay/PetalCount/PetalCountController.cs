using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PetalCountController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private RectTransform _navbar;
    [SerializeField] private Button _navbarBtn;
    [SerializeField] private int _petalCount;
    [SerializeField] private float _time = 0.5f;
    [SerializeField] private Sprite _hide;
    [SerializeField] private Sprite _show;
    private readonly WaitForSeconds _wait = new WaitForSeconds(0.5f);
    private readonly Vector2 _closePos = new Vector2(-225, 0);
    private readonly Vector2 _openPos = new Vector2(0, 0);
    private bool _locked;
    private int _count;
    private bool _open;

    private void OnEnable()
    {
        SetupLevel();
        GameEventManager.PetalCount += Check;
    }

    private void OnDisable()
    {
        GameEventManager.PetalCount -= Check;
    }

    private void Check(int value)
    {
        _count += value;
        if (_count >= _petalCount)
        {
            if (ResourceManager.PetalCount < 4)
            {
                ResourceManager.PetalCount++;
            }
            else
            {
                ResourceManager.PetalCount = 1;
            }

            StartCoroutine(NewLevel());
        }
    }

    private IEnumerator NewLevel()
    {
        yield return _wait;
        GameUIManager.Instance.ScreenShot();
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }

    public void NextLevel(int index)
    {
        if (index == 11)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }

        if (!_locked)
        {
            ResourceManager.PetalCount = index;
            GameUIManager.Instance.Reload();
        }
        else
        {
            Debug.LogError("Watch Ads");
            //GameUIManager.Instance.WatchAds();
        }
    }

    private void SetupLevel()
    {
        _count = 0;
        _open = true;
        _navbar.anchoredPosition = _openPos;
        int num = ResourceManager.PetalCount - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }

    public void SetupNavbar()
    {
        if (_open)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        _open = true;
        _navbar.DOAnchorPos(_openPos, _time).OnComplete(() =>
        {
            _navbarBtn.image.sprite = _hide;
        });
    }

    private void Hide()
    {
        _open = false;
        _navbar.DOAnchorPos(_closePos, _time).OnComplete(() =>
        {
            _navbarBtn.image.sprite = _show;
        });
    }
}