using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> _itemList = new List<Item>();
    public List<int> _quantityList = new List<int>();

    public GameObject _inventoryPanel;

    List<InventorySlot> _slotList = new List<InventorySlot>();

    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        foreach(InventorySlot child in _inventoryPanel.GetComponentsInChildren<InventorySlot>())
        {
            _slotList.Add(child);
        }
    }

    public void AddItem(Item _itemAdded, int _quantityAdded)
    {
        if (_itemAdded._itemStackable)
        {
            if (_itemList.Contains(_itemAdded))
            {
                _quantityList[_itemList.IndexOf(_itemAdded)] = _quantityList[_itemList.IndexOf(_itemAdded)] + _quantityAdded;
            }
            else
            {
                if (_itemList.Count < _slotList.Count)
                {
                    _itemList.Add(_itemAdded);
                    _quantityList.Add(_quantityAdded);
                }
            }
        }
        else
        {
            for (int i = 0; i <= _quantityAdded; i++)
            {
                if (_itemList.Count < _slotList.Count)
                {
                    _itemList.Add(_itemAdded);
                    _quantityList.Add(1);
                }
            }
        }

        UpdateInventoryUI();
    }

    public void RemoveItem(Item _itemRemoved, int _quantityRemoved)
    {
        if (_itemRemoved._itemStackable)
        {
            if(_itemList.Contains(_itemRemoved))
            {
                _quantityList[_itemList.IndexOf(_itemRemoved)] = _quantityList[_itemList.IndexOf(_itemRemoved)] - _quantityRemoved;

                if(_quantityList[_itemList.IndexOf(_itemRemoved)] <= 0)
                {
                    _quantityList.RemoveAt(_itemList.IndexOf(_itemRemoved));
                    _itemList.RemoveAt(_itemList.IndexOf(_itemRemoved));
                }
            }
        }
        else
        {
            for(int i = 0; i < _quantityRemoved; i++)
            {
                _quantityList.RemoveAt(_itemList.IndexOf(_itemRemoved));
                _itemList.RemoveAt(_itemList.IndexOf(_itemRemoved));
            }
        }

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        int idx = 0;

        foreach(InventorySlot slot in _slotList)
        {
            if(_itemList.Count != 0)
            {
                if(idx < _itemList.Count)
                {
                    slot.UpdateSlot(_itemList[idx], _quantityList[idx]);
                    idx++;
                }
                else
                {
                    slot.UpdateSlot(null, 0);
                }
            }
            else
            {
                slot.UpdateSlot(null, 0);
            }
        }
    }
}
