using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.PlayerLoop;

[Serializable]
public class Reward
{
    public int _exp;
    public int _gold;
}
public class Quest : ScriptableObject
{
    public enum Type { DIALOGUE, HUNTING }

    public int _questId = 101;
    public string _title;
    public Type _type;
    public NPC _startNPC;
    public string _desc;
    public List<string> _dialogues = new List<string>();
    public Reward _reward;
    public int _requiredLevel = 1;
    public Quest _precednetQuest;

    public bool CanAccept()
    {
        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        QuestManager questManager = PlayerController.instance.QuestManager;
        if (playerManager._playerLv >= _requiredLevel)
        {
            if (_precednetQuest == null)
                return true;
            else
            {
                List<Quest> completedQuests = questManager.GetCompletedQuest();
                if (completedQuests.Any(item => item._questId == _precednetQuest._questId))
                    return true;
            }
        }
        return false;
    }
}