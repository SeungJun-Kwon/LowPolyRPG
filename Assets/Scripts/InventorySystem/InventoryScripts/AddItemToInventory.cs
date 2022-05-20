using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToInventory : MonoBehaviour
{
    public bool _specificItemGive, _equipItemGive;

    public List<Item> _itemsToGive = new List<Item>();

    public Item _specificItem;
    public int _specificQuant;

    public void AddItem()
    {
        if(_specificItemGive && !_equipItemGive)
        {
            AddSpecificItem();
        }
        else if(!_specificItemGive && _equipItemGive)
        {
            AddEquipItem();
        }
    }

    void AddSpecificItem()
    {
        Inventory.instance.AddItem(_specificItem, _specificQuant);
    }

    void AddEquipItem()
    {
        Inventory.instance.AddItem(_itemsToGive[Random.Range(0, _itemsToGive.Count)], Random.Range(1, 5));
    }
}
