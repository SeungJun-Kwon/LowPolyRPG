using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    PlayerManager _playerManager;

    List<QuestData> _currentQuest = new List<QuestData>();
    List<QuestData> _completedQuest = new List<QuestData>();

    public bool FindQuest(Quest quest)
    {
        foreach (var item in _currentQuest)
            if (item._quest._questId == quest._questId)
                return true;
        return false;
    }

    public QuestData GetQuest(Quest quest)
    {
        foreach (var item in _currentQuest)
            if (item._quest._questId == quest._questId)
                return item;
        return null;
    }

    public List<QuestData> GetCurrentQuestData()
    {
        return _currentQuest;
    }

    public List<QuestData> GetCurrentQuestData(Quest.Type type)
    {
        List<QuestData> result = new List<QuestData>();

        foreach(var item in _currentQuest)
            if (item._quest._type == type)
                result.Add(item);

        if (result.Count == 0)
            return null;

        return result;
    }

    public List<Quest> GetCurrentQuest()
    {
        List<Quest> result = new List<Quest>();

        foreach (var item in _currentQuest)
            result.Add(item._quest);

        return result;
    }

    public List<QuestData> GetCompletedQuestData()
    {
        return _completedQuest;
    }

    public List<Quest> GetCompletedQuest()
    {
        List<Quest> result = new List<Quest>();

        foreach (var item in _completedQuest)
            result.Add(item._quest);

        return result;
    }

    public void AcceptQuest(Quest quest)
    {
        QuestData questData = null;

        if (quest._type == Quest.Type.HUNTING)
        {
            questData = new HuntingQuestData(quest as HuntingQuest);
        }
        else if (quest._type == Quest.Type.DIALOGUE)
        {
            questData = new DialogueQuestData(quest as DialogueQuest);
        }
        else
        {
            Debug.Log("!!!Check Quest Type!!!");
            return;
        }

        _currentQuest.Add(questData);
    }

    public void CompleteQuest(Quest quest)
    {
        for (int i = 0; i < _currentQuest.Count; i++)
        {
            if (_currentQuest[i]._quest._questId == quest._questId)
            {
                _completedQuest.Add(_currentQuest[i]);
                _currentQuest.RemoveAt(i);
                PlayerController.instance.PlayerManager.GainExp(quest._reward._exp);
                //PlayerController.instance.PlayerManager.GainGold(quest._reward._gold);
                return;
            }
        }
    }
}
