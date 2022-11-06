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

    [Header("�� NPC �� ��ȭ\n�� �ݵ�� targetNPC�� ������ ���ƾ� �� ��")]
    [SerializeField] public List<NPCDialogue> _npcDialogue = new List<NPCDialogue>();

    [HideInInspector] public int _currentIndex = 0;
    bool[] _done;

    private void Awake()
    {
        this._type = Type.DIALOGUE;

        _done = new bool[_targetNPC.Length];
    }

    public void CanComplete()
    {
        foreach(var b in _done)
            if (b == false)
                return;

        _canComplete = true;
    }
}
