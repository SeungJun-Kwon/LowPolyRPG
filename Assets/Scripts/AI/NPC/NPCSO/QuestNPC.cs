using UnityEngine;

[CreateAssetMenu(fileName = "QuestNPC", menuName = "NPC/QuestNPC")]
public class QuestNPC : NPC
{
    public Quest[] _quest;

    private void Awake()
    {
        this._type = Type.QuestNPC;
    }
}
