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
            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "��ȭ ����");
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
            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "����Ʈ ���");
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
        _npc._name = EditorGUILayout.TextField("�̸�", _npc._name);
        _npc._type = (NPC.Type)EditorGUILayout.EnumPopup("NPC Ÿ��", _npc._type);
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        _npc._isTimeline = EditorGUILayout.Toggle("Ÿ�Ӷ��� ����", _npc._isTimeline);
        if(_npc._isTimeline)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ÿ�Ӷ���");
            EditorGUILayout.Space();
            _npc._playableDirector = (PlayableDirector)EditorGUILayout.ObjectField("Playable Director", _npc._playableDirector, typeof(PlayableDirector), true);
            _npc._timeline = (TimelineAsset)EditorGUILayout.ObjectField("Timeline", _npc._timeline, typeof(TimelineAsset), true);
        }
    }
}
