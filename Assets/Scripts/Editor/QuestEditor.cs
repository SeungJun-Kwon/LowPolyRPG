using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(Quest), true)]
public class QuestEditor : Editor
{
    Quest _quest;

    private void OnEnable()
    {
        _quest = (Quest)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _quest._isTimeline = EditorGUILayout.Toggle("타임라인 유무", _quest._isTimeline);
        if(_quest._isTimeline)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("타임라인");
            EditorGUILayout.Space();
            _quest._playableDirector = (PlayableDirector)EditorGUILayout.ObjectField("Playable Director", _quest._playableDirector, typeof(PlayableDirector), true);
            _quest._timeline = (TimelineAsset)EditorGUILayout.ObjectField("Timeline", _quest._timeline, typeof(TimelineAsset), true);
        }
    }
}
