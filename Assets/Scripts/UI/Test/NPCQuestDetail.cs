using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestDetail : MonoBehaviour
{
    Text _title, _desc, _reward;

    Questsample _quest;
    NPCDialoguesample _npcDialoguesample;

    private void Awake()
    {
        transform.Find("Title").gameObject.TryGetComponent<Text>(out _title);
        transform.Find("Description").gameObject.TryGetComponent<Text>(out _desc);
        transform.Find("Reward").gameObject.TryGetComponent<Text>(out _reward);

        UIController.instance.transform.Find("NPCDialoguesample").gameObject.TryGetComponent<NPCDialoguesample>(out _npcDialoguesample);
    }

    public void SetQuest(Questsample quest)
    {
        _quest = quest;
        _title.text = _quest._title;
        _desc.text = _quest._desc;
        _reward.text = "º¸»ó | Gold : " + _quest._reward._gold + ", Exp : " + _quest._reward._exp;
    }

    public void AcceptQuest()
    {
        PlayerController.instance.PlayerManager.AcceptQuest(_quest);
        _npcDialoguesample.SetState(0);
    }
}
