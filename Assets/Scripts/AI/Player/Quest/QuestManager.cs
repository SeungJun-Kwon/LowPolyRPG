using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    PlayerManager _playerManager;

    List<QuestData> _currentQuest = new List<QuestData>();
    List<QuestData> _completedQuest = new List<QuestData>();

    AudioClip _questClearSound;

    private void Start()
    {
        _questClearSound = Resources.Load("Sounds/SFX/SFX_QuestClear") as AudioClip;
    }

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

        return result;
    }

    public List<QuestData> GetCurrentQuestData(NPC npc)
    {
        List<QuestData> result = new List<QuestData>();

        foreach (QuestData item in _currentQuest)
            if (item._quest._startNPC == npc)
                result.Add(item);

        result.Sort((x, y) => x._quest._questId.CompareTo(y._quest._questId));

        return result;
    }

    public List<Quest> GetCurrentQuest()
    {
        List<Quest> result = new List<Quest>();

        foreach (var item in _currentQuest)
            result.Add(item._quest);

        return result;
    }

    public List<Quest> GetCurrentQuest(NPC npc)
    {
        List<Quest> result = new List<Quest>();

        foreach (QuestData item in _currentQuest)
            if (item._quest._startNPC == npc)
                result.Add(item._quest);

        result.Sort((x, y) => x._questId.CompareTo(y._questId));

        return result;
    }

    public bool CurrentQuestContain(Quest quest)
    {
        foreach (var item in _currentQuest)
            if (item._quest._questId == quest._questId)
                return true;

        return false;
    }

    public List<QuestData> GetCompletedQuestData()
    {
        return _completedQuest;
    }

    public List<QuestData> GetCompletedQuestData(NPC npc)
    {
        List<QuestData> result = new List<QuestData>();

        foreach (QuestData item in _completedQuest)
            if (item._quest._startNPC == npc)
                result.Add(item);

        result.Sort((x, y) => x._quest._questId.CompareTo(y._quest._questId));

        return result;
    }

    public List<Quest> GetCompletedQuest()
    {
        List<Quest> result = new List<Quest>();

        foreach (var item in _completedQuest)
            result.Add(item._quest);

        return result;
    }

    public List<Quest> GetCompletedQuest(NPC npc)
    {
        List<Quest> result = new List<Quest>();

        foreach (QuestData item in _completedQuest)
            if (item._quest._startNPC == npc)
                result.Add(item._quest);

        result.Sort((x, y) => x._questId.CompareTo(y._questId));

        return result;
    }

    public bool CompletedQuestContain(Quest quest)
    {
        foreach(var item in _completedQuest)
            if (item._quest._questId == quest._questId)
                return true;

        return false;
    }

    public void AcceptQuest(QuestData questData)
    {
        _currentQuest.Add(questData);
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

    public void CompleteQuest(Quest quest, UnityAction unityAction)
    {
        for (int i = 0; i < _currentQuest.Count; i++)
        {
            if (_currentQuest[i]._quest._questId == quest._questId)
            {
                _currentQuest[i].OnQuestComplete.RemoveListener(unityAction);
                _completedQuest.Add(_currentQuest[i]);
                _currentQuest.RemoveAt(i);
                PlayerController.instance.PlayerManager.CurrentExp += quest._reward._exp;
                //PlayerController.instance.PlayerManager.GainGold(quest._reward._gold);
                SoundManager.instance.SFXPlay(_questClearSound);
                return;
            }
        }
    }
}
