using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool _canAccept = false;
    public bool _canComplete = false;
    public int _requiredLevel = 1;
    public Questsample _precednetQuest;

    private void Awake()
    {
        CanAccept();
    }

    public bool CanAccept()
    {
        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        if (playerManager._playerLv >= _requiredLevel)
        {
            if (_precednetQuest == null)
                return true;
            else
            {
                List<Questsample> completedQuests = playerManager.GetCompletedQuestssample();
                if (completedQuests.Contains(_precednetQuest))
                    return true;
            }
        }
        return false;
    }

    public void QuestComplete()
    {
        if( _canComplete)
        {
            int index = _startNPC._quests.IndexOf(this);
            _startNPC._quests.RemoveAt(index);
        }
    }
}