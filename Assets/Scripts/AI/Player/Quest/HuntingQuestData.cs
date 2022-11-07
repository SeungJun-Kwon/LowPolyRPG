using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingQuestData : QuestData
{
    public int _currentNumberOfHunts;

    public HuntingQuestData(HuntingQuest quest) : base(quest)
    {
        _quest = quest;
        _currentNumberOfHunts = 0;
    }

    public void HuntMonster(Monster monster)
    {
        HuntingQuest quest = _quest as HuntingQuest;

        if (quest._targetMonster != monster)
            return;

        if (_currentNumberOfHunts < quest._numberOfHunts)
        {
            _currentNumberOfHunts++;
            UIController.instance.NoticeArea.GetMessage(quest._targetMonster.name + " " + _currentNumberOfHunts + "/" + quest._numberOfHunts);
            if (_currentNumberOfHunts == quest._numberOfHunts)
                _canComplete = true;
        }
    }
}
