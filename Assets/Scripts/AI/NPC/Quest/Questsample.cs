using System.Collections.Generic;
using UnityEngine;

public class Questsample : ScriptableObject
{
    public enum Type { DIALOGUE, HUNTING }

    public string _title;
    public Type _type;
    public NPCsample _startNPC;
    public string _desc;
    public List<string> _dialogues = new List<string>();
    public Reward _reward;
    public bool _canComplete = false;
    public int _requiredLevel = 1;
    public Quest _precednetQuest;
}