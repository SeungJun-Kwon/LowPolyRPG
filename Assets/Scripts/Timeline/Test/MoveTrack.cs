using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(MoveAsset))]
[TrackBindingType(typeof(Transform))]
public class MoveTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MoveMixerBehavior>.Create(graph, inputCount);
    }
}
