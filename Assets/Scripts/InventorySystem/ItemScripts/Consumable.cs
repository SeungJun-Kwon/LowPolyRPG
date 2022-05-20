using UnityEngine;
[CreateAssetMenu(fileName = "Consumable", menuName = "Item/Consumable")]

public class Consumable : Item
{
    public enum ConsumableType { Potion, }

    public ConsumableType _type;
    public float _effectValueOfTheItem;

    public override void Use(int _useNumber)
    {
        base.Use();

        //Consumble Action

        // Inventory.instance.RemoveItem(this, _useNumber);
    }
}
