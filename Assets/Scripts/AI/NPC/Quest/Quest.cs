using UnityEngine;

[System.Serializable]
public class Reward
{
    public int _exp;
    public int _gold;
}

public class Quest : ScriptableObject
{
    public enum Type { DIALOGUE, HUNTING }

    public string _title;
    public Type _type;
    public NPC _startNPC;
    public string _desc;
    public Reward _reward;
    public bool _canComplete = false;
    public int _requiredLevel = 1;
}
