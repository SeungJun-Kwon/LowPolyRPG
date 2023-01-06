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

        _quest._isTimeline = EditorGUILayout.Toggle("Ÿ�Ӷ��� ����", _quest._isTimeline);
        if(_quest._isTimeline)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ÿ�Ӷ���");
            EditorGUILayout.Space();
            _quest._playableDirector = (PlayableDirector)EditorGUILayout.ObjectField("Playable Director", _quest._playableDirector, typeof(PlayableDirector), true);
            _quest._timeline = (TimelineAsset)EditorGUILayout.ObjectField("Timeline", _quest._timeline, typeof(TimelineAsset), true);
        }
    }
}
