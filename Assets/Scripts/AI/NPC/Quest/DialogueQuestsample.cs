using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueQuestsample", menuName = "Questsample/DialogueQuestsample")]
public class DialogueQuestsample : Questsample
{
    public NPCsample[] _targetNPC;

    [Serializable]
    public struct NPCDialogue
    {
        public string[] _sentences;
    }

    [Header("�� NPC �� ��ȭ\n�� �ݵ�� targetNPC�� ������ ���ƾ� �� ��")][SerializeField]
    public List<NPCDialogue> _npcDialogue = new List<NPCDialogue>();

    private void Awake()
    {
        this._type = Type.DIALOGUE;
    }
}
