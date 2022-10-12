using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NPC : ScriptableObject
{
    public enum Type { QuestNPC, DialogueNPC, ShopNPC, };

    public string _name;
    public Type _type;
    public bool _isTimeline = false;
    public PlayableDirector _playableDirector = null;
    public TimelineAsset _timeline = null;
}
