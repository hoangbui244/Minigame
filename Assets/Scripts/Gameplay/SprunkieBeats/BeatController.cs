using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeatController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum BeatType
    {
        Beat1 = 1,
        Beat2 = 2,
        Beat3 = 3,
        Beat4 = 4,
        Beat5 = 5,
        Beat6 = 6,
        Beat7 = 7,
        Beat8 = 8,
        Beat9 = 9,
        Beat10 = 10,
        Beat11 = 11,
        Beat12 = 12,
        Beat13 = 13,
        Beat14 = 14,
        Beat15 = 15,
        Beat16 = 16,
        Beat17 = 17,
        Beat18 = 18,
        Beat19 = 19,
        Beat20 = 20,
    }
    [Header("======== Beat Type ========")]
    public BeatType Type;
    
    [Header("======== Set Parent ========")]
    [SerializeField] private Transform _setParent;
    private Transform _parentAfterDrag;

    [Header("======== Other ========")] 
    [SerializeField] private Color _color;
    private Color _defaultColor;
    private Image _image;
    private CharController _currentHover;
    private bool _isAssigned;

    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        _isAssigned = false;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isAssigned) return;
        _parentAfterDrag = transform.parent;
        transform.SetParent(_setParent);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAssigned) return;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var globalMousePos);
        transform.position = globalMousePos;

        RaycastHit2D hit = Physics2D.Raycast(globalMousePos, Vector2.zero);
        if (hit.collider != null)
        {
            var character = hit.collider.GetComponent<CharController>();
            if (character != null && character != _currentHover)
            {
                _currentHover?.SetHover(false);
                _currentHover = character;
                _currentHover.SetHover(true);
            }
        }
        else
        {
            _currentHover?.SetHover(false);
            _currentHover = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAssigned) return;
        if (_currentHover != null)
        {
            if (_currentHover.Type != CharController.CharType.Default)
            {
                GameEventManager.UnselectCharacter?.Invoke((int)_currentHover.Type);
            }
            _currentHover.Type = (CharController.CharType)Type;
            _currentHover.CharacterSelected(true);
            _isAssigned = true;
            _image.color = _color;
            GameEventManager.Play?.Invoke();
        }
        else
        {
            ResetBeat();
        }
        
        _currentHover?.SetHover(false);
        _currentHover = null;
        transform.SetParent(_parentAfterDrag);
    }
    
    public void ResetBeat()
    {
        _image.color = _defaultColor;
        _isAssigned = false;
    }
}