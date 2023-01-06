using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TestAsset : PlayableAsset
{
    public TestBehavior _template;
    public ExposedReference<Transform> _startLocation;
    public ExposedReference<Transform> _endLocation;
    public ExposedReference<Transform> _lookRotation;
    public float _speed = 1f;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TestBehavior>.Create(graph, _template);
        TestBehavior clone = playable.GetBehaviour();
        clone._startLocation = _startLocation.Resolve(graph.GetResolver());
        clone._endLocation = _endLocation.Resolve(graph.GetResolver());
        clone._lookRotation = _lookRotation.Resolve(graph.GetResolver());
        clone._speed = _speed;
        return playable;
    }
}
