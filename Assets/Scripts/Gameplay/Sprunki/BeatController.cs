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
    private CharacterController _currentHover;

    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(_setParent);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var globalMousePos);
        transform.position = globalMousePos;

        RaycastHit2D hit = Physics2D.Raycast(globalMousePos, Vector2.zero);
        if (hit.collider != null)
        {
            var character = hit.collider.GetComponent<CharacterController>();
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
        if (_currentHover != null)
        {
            _currentHover.Type = (CharacterController.CharType)Type;
            _currentHover.CharacterSelected(true);
            _image.color = _color;
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
    }
}