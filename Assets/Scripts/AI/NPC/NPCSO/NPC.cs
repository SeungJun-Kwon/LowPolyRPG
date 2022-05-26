using UnityEngine;

public class NPC : ScriptableObject
{
    public enum Type { QuestNPC, DialogueNPC, ShopNPC, };

    public string _name;
    public Type _type;
}
