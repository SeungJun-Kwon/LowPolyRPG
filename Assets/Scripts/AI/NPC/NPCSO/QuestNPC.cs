using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestNPC", menuName = "NPC/QuestNPC")]
public class QuestNPC : NPC
{
    public List<Quest> _quest;

    private void Awake() => this._type = Type.QuestNPC;
}
