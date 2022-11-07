using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPC")]
public class NPC : ScriptableObject
{
    public string _name;
    public string _firstSentence;
    public List<string> _dialogues = new List<string>();
    public List<Quest> _quests = new List<Quest>();
    [HideInInspector]
    public bool _isTimeline = false;
    [HideInInspector]
    public PlayableDirector _playableDirector;
    [HideInInspector]
    public TimelineAsset _timeline;
}
