using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "NPCsample", menuName = "NPCsample/NPCsample")]
public class NPCsample : ScriptableObject
{
    public string _name;
    public string _firstSentence;
    public List<string> _dialogues = new List<string>();
    public List<Questsample> _quests = new List<Questsample>();
    [HideInInspector]
    public bool _isTimeline = false;
    [HideInInspector]
    public PlayableDirector _playableDirector;
    [HideInInspector]
    public TimelineAsset _timeline;
}
