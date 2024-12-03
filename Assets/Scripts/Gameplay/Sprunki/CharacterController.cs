using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public enum CharType
    {
        Default,
        Char1 = 1,
        Char2 = 2,
        Char3 = 3,
    }
    [Header("======== Char Type ========")]
    public CharType Type;
    
    [Header("======== Char Color ========")]
    [SerializeField] private Color _hoverColor;
    private Color _defaultColor;
    private Image _image;

    [Header("======== Other ========")] 
    public int ID;
    [SerializeField] private GameObject _board;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    public void SetHover(bool isHovering)
    {
        _image.color = isHovering ? _hoverColor : _defaultColor;
    }
    
    public void CharacterSelected(bool isOn)
    {
        _board.SetActive(isOn);
    }
}