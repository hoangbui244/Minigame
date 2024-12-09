using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteFromAtlas : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _atlas;
    [SerializeField] private string _spriteName;
    
    private void Start()
    {
        GetComponent<Image>().sprite = _atlas.GetSprite(_spriteName);
    }
    
    public void SetSprite(int index)
    {
        GetComponent<Image>().sprite = _atlas.GetSprite(index.ToString());
    }
}
