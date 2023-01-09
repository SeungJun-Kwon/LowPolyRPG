using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestData
{
    [HideInInspector] public Quest _quest;
    [HideInInspector] public UnityEvent OnQuestComplete = new UnityEvent();

    bool _canComplete;
    public bool CanComplete
    {
        get
        {
            return _canComplete;
        }
        set
        {
            _canComplete = value;
            if (CanComplete)
                OnQuestComplete.Invoke();
        }
    }

    public QuestData(Quest quest)
    {
        _quest = quest;
        CanComplete = false;
    }
}
