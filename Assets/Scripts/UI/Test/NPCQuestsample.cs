using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestsample : MonoBehaviour
{
    Text _name;

    KeyValuePair<Questsample, int> _quest;

    NPCDialoguesample _npcDialoguesample;
    PlayerManager _playerManager;

    private void Awake()
    {
        TryGetComponent<Text>(out _name);

        UIController.instance.transform.Find("NPCDialoguesample").gameObject.TryGetComponent<NPCDialoguesample>(out _npcDialoguesample);
    }

    public void SetQuest(Questsample quest, int progress)
    {
        _quest = new KeyValuePair<Questsample, int>(quest, progress);
        _name.text = _quest.Key._title;

        // 0 : 진행 중, 1 : 수락 가능, 2 : 수락 불가, 3 : 완료 가능
        if (progress == 0)
            _name.text += " (진행 중)";
        else if(progress == 1)
            _name.text += " (수락 가능)";
        else
            _name.text += " (수락 불가)";
    }

    public void OnClickQuest()
    {
        _playerManager = PlayerController.instance.PlayerManager;

        if (!_playerManager.FindQuestsample(_quest.Key))
        {
            _npcDialoguesample._selectedQuest = _quest.Key;
            _npcDialoguesample.SetState(3);
        }
        else
        {
            if(_quest.Key._type == Questsample.Type.HUNTING)
            {
                _npcDialoguesample._selectedQuest = _quest.Key;
                _npcDialoguesample.SetState(3);
            }
            else
            {
                DialogueQuestsample quest = (DialogueQuestsample)_playerManager.GetQuest(_quest.Key);
                NPCsample npc = _npcDialoguesample._npc;

                if (quest._targetNPC[quest._currentIndex] == npc)
                {
                    _npcDialoguesample.SetQuestDialogue(_quest.Key._dialogues);
                    _npcDialoguesample.SetState(4);
                }
            }
        }
    }
}
