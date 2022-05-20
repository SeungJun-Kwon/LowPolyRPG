using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]

public class Weapon : Item
{
    public enum WeaponType { Sword, }

    public WeaponType _type;
    public int _weaponDamage;

    public override void Use()
    {
        base.Use();

        // Equip Action
        // The Equip Action is avaible in the "Inventory & Equip System - Drag & Drop", an extension of this package that can be purchased from the Unity asset Store in the following link:
        // https://assetstore.unity.com/packages/slug/209478

        // Use the following line if you want to destroy this type of item after use
        // Inventory.instance.RemoveItem(this, 1);
    }
}
