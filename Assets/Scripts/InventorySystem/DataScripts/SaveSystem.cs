using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public List<Item> _itemLibrary = new List<Item>();

    string _inventoryString = "";

    public void TransformDataToString()
    {
        foreach (Item item in Inventory.instance._itemList)
        {
            _inventoryString = _inventoryString + item._itemID + ":" + Inventory.instance._quantityList[Inventory.instance._itemList.IndexOf(item)] + "/";
        }
    }

    public void SaveInventory()
    {
        TransformDataToString();
        string _destination = Application.persistentDataPath + "/save.dat";
        FileStream _file;

        if (File.Exists(_destination))
            _file = File.OpenWrite(_destination);
        else
            _file = File.Create(_destination);

        InventoryData _data = new InventoryData(_inventoryString);
        BinaryFormatter _bf = new BinaryFormatter();
        _bf.Serialize(_file, _data);
        _file.Close();
    }

    public void LoadInventory()
    {
        _inventoryString = "";
        string _destination = Application.persistentDataPath + "/save.dat";
        FileStream _file;

        if (File.Exists(_destination))
            _file = File.OpenRead(_destination);
        else
            return;

        BinaryFormatter _bf = new BinaryFormatter();
        InventoryData _data = (InventoryData)_bf.Deserialize(_file);
        _file.Close();

        ReadInventoryData(_data._inventoryString);

        Inventory.instance.UpdateInventoryUI();
    }

    public void ReadInventoryData(string _data)
    {
        Inventory.instance._itemList.Clear();
        Inventory.instance._quantityList.Clear();

        string[] _splitData = _data.Split(char.Parse("/"));

        foreach (string stg in _splitData)
        {
            string[] splitID = stg.Split(char.Parse(":"));

            if(splitID.Length >= 2)
            {
                Inventory.instance._itemList.Add(_itemLibrary[int.Parse(splitID[0])]);
                Inventory.instance._quantityList.Add(int.Parse(splitID[1]));
            }
        }
    }
}
