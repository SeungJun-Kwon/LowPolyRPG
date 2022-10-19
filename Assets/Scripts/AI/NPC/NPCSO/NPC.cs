using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NPC : ScriptableObject
{
    public enum Type { QuestNPC, DialogueNPC, ShopNPC, };

    public string _name;
    public Type _npcType;
    public bool _isTimeline = false;
    public PlayableDirector _playableDirector;
    public TimelineAsset _timeline;
}
