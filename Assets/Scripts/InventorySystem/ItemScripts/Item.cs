using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string _itemName;
    public int _itemID;
    public int _itemPrice;
    public bool _itemStackable;
    public Sprite _itemSprite;
    public string _itemDescription;

    public virtual void Use()
    {

    }

    public virtual void Use(int _useNum)
    {

    }
}
