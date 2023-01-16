using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueQuest", menuName = "Quest/DialogueQuest")]
public class DialogueQuest : Quest
{
    public NPC[] _targetNPC;

    [Serializable]
    public struct NPCDialogue
    {
        public List<string> _sentences;
    }

    [Header("각 NPC 별 대화\n※ 반드시 targetNPC와 개수가 같아야 함 ※")]
    [SerializeField] public List<NPCDialogue> _npcDialogue = new List<NPCDialogue>();

    protected override void Awake()
    {
        base.Awake();
        this._type = Type.DIALOGUE;
    }
}
