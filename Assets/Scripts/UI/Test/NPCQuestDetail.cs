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

        UIController.instance.transform.Find("NPCDialogue").gameObject.TryGetComponent<NPCDialogue>(out _npcDialogue);
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
        PlayerController.instance.QuestManager.AcceptQuest(_quest);
        _npcDialogue.SetState((int)NPCDialogueState.INIT);
    }

    public void CompleteQuest()
    {
        QuestManager questManager = PlayerController.instance.QuestManager;
        questManager.CompleteQuest(_quest);
        _npcDialogue.SetState((int)NPCDialogueState.INIT);
    }
}
