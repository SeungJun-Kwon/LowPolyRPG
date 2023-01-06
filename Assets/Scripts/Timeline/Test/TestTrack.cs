using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(TestAsset))]
[TrackBindingType(typeof(Transform))]
public class TestTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TestMixerBehavior>.Create(graph, inputCount);
    }
}
