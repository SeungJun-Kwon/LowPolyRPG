using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(NPC), true)]
public class NPCEditor : Editor
{
    NPC _npc;
    ReorderableList list = null;

    private void OnEnable()
    {
        _npc = (NPC)target;

        if (_npc._type == NPC.Type.DialogueNPC)
        {
            DialogueNPC dialogueNPC = (DialogueNPC)target;
            var properties = serializedObject.FindProperty("_dialogue");
            list = new ReorderableList(serializedObject, properties);
            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "대화 내용");
            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var elements = properties.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, elements);
            };
        }
        else if(_npc._type == NPC.Type.QuestNPC)
        {
            QuestNPC questNPC = (QuestNPC)target;
            var properties = serializedObject.FindProperty("_quest");
            list = new ReorderableList(serializedObject, properties);
            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "퀘스트 목록");
            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var elements = properties.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, elements);
            };
        }
    }

    public override void OnInspectorGUI()
    {
        _npc._name = EditorGUILayout.TextField("이름", _npc._name);
        _npc._type = (NPC.Type)EditorGUILayout.EnumPopup("NPC 타입", _npc._type);
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

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
