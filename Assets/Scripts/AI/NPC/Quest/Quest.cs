using UnityEngine;

[System.Serializable]
public class Reward
{
    public int _exp;
    public int _gold;
}

[CreateAssetMenu(fileName = "Quest", menuName = "NPC/Quest")]
public class Quest : ScriptableObject
{
    public enum Type { MAIN, SUB, }

    public string _title;
    public Type _type;
    public string _desc;
    public Reward _reward;
}
