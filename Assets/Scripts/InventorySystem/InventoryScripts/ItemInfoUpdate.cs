using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUpdate : MonoBehaviour
{
    public GameObject _infoPanel;

    public Text _nameText;
    public Image _icon;
    public Text _descriptionText;

    public void UpdateInfoPanel(Item _itemInfo, Vector2 _mousePosition)
    {
        if(_itemInfo != null)
        {
            _infoPanel.SetActive(true);
            var _pos = Camera.main.ScreenToWorldPoint(_mousePosition);
            _infoPanel.transform.position = new Vector3(_pos.x + 200f, _pos.y, _pos.z);

            _nameText.text = _itemInfo._itemName;
            _icon.sprite = _itemInfo._itemSprite;
            _descriptionText.text = _itemInfo._itemDescription;
        }
        else
        {
            _infoPanel.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        _infoPanel.SetActive(false);
    }
}
