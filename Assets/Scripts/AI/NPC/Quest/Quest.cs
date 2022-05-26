using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "NPC/Quest")]
public class Quest : ScriptableObject
{
    public string _title;
    public string _desc;
    public string[] _reward;
}
