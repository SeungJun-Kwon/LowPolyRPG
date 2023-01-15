using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestDetail : MonoBehaviour
{
    Text _title, _desc, _reward;

    Quest _quest;
    NPCDialogue _npcDialogue;

    private void Awake()
    {
        transform.Find("Title").gameObject.TryGetComponent<Text>(out _title);
        transform.Find("Description").gameObject.TryGetComponent<Text>(out _desc);
        transform.Find("Reward").gameObject.TryGetComponent<Text>(out _reward);
    }

    private void Start()
    {
        UIController.instance.PlayUI.transform.Find("NPCDialogue").gameObject.TryGetComponent<NPCDialogue>(out _npcDialogue);
    }

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        _title.text = _quest._title;
        _desc.text = _quest._desc;
        _reward.text = "º¸»ó | Gold : " + _quest._reward._gold + ", Exp : " + _quest._reward._exp;
    }

    public void AcceptQuest()
    {
        QuestData questData = null;

        if (_quest._type == Quest.Type.HUNTING)
        {
            questData = new HuntingQuestData(_quest as HuntingQuest);
        }
        else if (_quest._type == Quest.Type.DIALOGUE)
        {
            questData = new DialogueQuestData(_quest as DialogueQuest);
        }
        else
        {
            Debug.Log("!!!Check Quest Type!!!");
            return;
        }
        questData.OnQuestComplete.AddListener(_npcDialogue._npcAI.CheckQuestState);
        PlayerController.instance.QuestManager.AcceptQuest(questData);
        _npcDialogue.SetState((int)NPCDialogueState.INIT);
    }

    public void CompleteQuest()
    {
        QuestManager questManager = PlayerController.instance.QuestManager;
        questManager.CompleteQuest(_quest, _npcDialogue._npcAI.CheckQuestState);
        _npcDialogue.gameObject.SetActive(false);
        try
        {
            if (_quest._isTimeline)
                TimelineController.instance.PlayFromTimeline(_quest._playableDirector, _quest._timeline);
        }catch(ArgumentNullException e)
        {
            Debug.Log(e.Message);
        }
    }
}
