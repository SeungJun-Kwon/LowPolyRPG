using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuest : MonoBehaviour
{
    Text _name;

    KeyValuePair<Quest, int> _quest;

    NPCDialogue _npcDialogue;
    QuestManager _questManager;

    private void Awake()
    {
        TryGetComponent<Text>(out _name);

        UIController.instance.transform.Find("NPCDialogue").gameObject.TryGetComponent<NPCDialogue>(out _npcDialogue);
    }

    public void SetQuest(Quest quest, int progress)
    {
        _quest = new KeyValuePair<Quest, int>(quest, progress);
        _name.text = _quest.Key._title;

        // 0 : 완료 가능, 1 : 진행 중, 2 : 수락 가능, 3 : 수락 불가
        if (progress == 0)
            _name.text += " (완료 가능)";
        else if (progress == 1)
            _name.text += " (진행 중)";
        else if (progress == 2)
            _name.text += " (수락 가능)";
        else
            _name.text += " (수락 불가)";
    }

    public void OnClickQuest()
    {
        _npcDialogue._selectedQuest = _quest.Key;

        _questManager = PlayerController.instance.QuestManager;
        QuestData quest = _questManager.GetQuest(_quest.Key);

        if (quest != null && quest._canComplete)
        {
            _npcDialogue.SetState((int)NPCDialogueState.QUESTDETAIL);
            return;
        }
        else if(_quest.Value == 3)
        {
            string s = string.Format("<선행 조건>\nLv : {0}\n선행 퀘스트 : {1}", _quest.Key._requiredLevel, _quest.Key._precednetQuest == null ? "없음" : _quest.Key._precednetQuest._title);
            _npcDialogue.SetState((int)NPCDialogueState.DIALOGUE);
            _npcDialogue.SetContentText(s);
            return;
        }

        if (_quest.Key._type == Quest.Type.DIALOGUE)
        {
            DialogueQuest dq = (DialogueQuest)_quest.Key;
            if(_questManager.FindQuest(_quest.Key))
            {
                DialogueQuestData dialogueQuest = quest as DialogueQuestData;
                NPC npc = _npcDialogue._npc;
                if (dq._targetNPC[dialogueQuest._currentIndex] == npc)
                {
                    _npcDialogue.SetQuestDialogue(dq._npcDialogue[dialogueQuest._currentIndex]._sentences);
                    _npcDialogue.SetState((int)NPCDialogueState.QUESTDIALOGUE);
                }
            }
            else
            {
                if (_quest.Key._startNPC == _npcDialogue._npc)
                {
                    _npcDialogue.SetQuestDialogue(_quest.Key._dialogues);
                    _npcDialogue.SetState((int)NPCDialogueState.QUESTDIALOGUE);
                }
            }
        }
        else if (_quest.Key._type == Quest.Type.HUNTING)
            _npcDialogue.SetState((int)NPCDialogueState.QUESTDETAIL);
    }
}
