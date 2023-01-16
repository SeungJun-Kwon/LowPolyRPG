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

    [Header("�� NPC �� ��ȭ\n�� �ݵ�� targetNPC�� ������ ���ƾ� �� ��")]
    [SerializeField] public List<NPCDialogue> _npcDialogue = new List<NPCDialogue>();

    protected override void Awake()
    {
        base.Awake();
        this._type = Type.DIALOGUE;
    }
}
