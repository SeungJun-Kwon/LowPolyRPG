using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestData : QuestData
{
    public new DialogueQuest _quest;
    public int _currentIndex;

    public DialogueQuestData(DialogueQuest quest) : base(quest)
    {
        _quest = quest;
        _currentIndex = 0;
    }

    public void IncreaseCurrentIndex()
    {
        DialogueQuest quest = _quest as DialogueQuest;
        _currentIndex += 1;
        if (_currentIndex >= quest._targetNPC.Length)
            CanComplete = true;
    }
}
