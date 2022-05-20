using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item _item;

    public Image _itemImage;
    public Text _quantity;

    public Button _removeButton;

    public void UpdateSlot(Item _itemInSlot, int _quantityInSlot)
    {
        _item = _itemInSlot;

        if(_itemInSlot != null && _quantityInSlot != 0)
        {
            _removeButton.enabled = true;
            _itemImage.enabled = true;
            _itemImage.sprite = _itemInSlot._itemSprite;

            if(_quantityInSlot > 1)
            {
                _quantity.enabled = true;
                _quantity.text = _quantityInSlot.ToString();
            }
            else
            {
                _quantity.enabled = false;
            }
        }
        else
        {
            _removeButton.enabled = false;
            _itemImage.enabled = false;
            _quantity.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        // Item Info UI Update
        GetComponentInParent<ItemInfoUpdate>().UpdateInfoPanel(_item, _eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Item Info UI Update
        GetComponentInParent<ItemInfoUpdate>().ClosePanel();
    }

    public void UseItem()
    {
        if(_item != null)
        {
            _item.Use();
        }
    }

    public void RemoveItem()
    {
        // Remove Item at Inventory
        Inventory.instance.RemoveItem(Inventory.instance._itemList[Inventory.instance._itemList.IndexOf(_item)], 1);
    }
}
