using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    [HideInInspector] public Quest _quest;

    public bool _canAccept;
    public bool _canComplete;

    public QuestData(Quest quest)
    {
        _quest = quest;
        _canAccept = _quest.CanAccept();
        _canComplete = false;
    }
}
