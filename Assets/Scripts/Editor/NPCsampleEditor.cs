using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(NPCsample), true)]
public class NPCsampleEditor : Editor
{
    NPCsample _npc;
    ReorderableList list = null;

    private void OnEnable()
    {
        _npc = (NPCsample)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _npc._isTimeline = EditorGUILayout.Toggle("타임라인 유무", _npc._isTimeline);
        if(_npc._isTimeline)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("타임라인");
            EditorGUILayout.Space();
            _npc._playableDirector = (PlayableDirector)EditorGUILayout.ObjectField("Playable Director", _npc._playableDirector, typeof(PlayableDirector), true);
            _npc._timeline = (TimelineAsset)EditorGUILayout.ObjectField("Timeline", _npc._timeline, typeof(TimelineAsset), true);
        }
    }
}
