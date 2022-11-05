using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Questsample : ScriptableObject
{
    public enum Type { DIALOGUE, HUNTING }

    public int _questId = 101;
    public string _title;
    public Type _type;
    public NPCsample _startNPC;
    public string _desc;
    public List<string> _dialogues = new List<string>();
    public Reward _reward;
    public bool _canComplete = false;
    public int _requiredLevel = 1;
    public Quest _precednetQuest;

    public void QuestComplete()
    {
        if( _canComplete)
        {
            int index = _startNPC._quests.IndexOf(this);
            _startNPC._quests.RemoveAt(index);
        }
    }
}