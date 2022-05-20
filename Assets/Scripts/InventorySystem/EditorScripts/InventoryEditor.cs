using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory)), CanEditMultipleObjects]

public class InventoryEditor : Editor
{
    SerializedProperty _inventoryPanel;

    private void OnEnable()
    {
        _inventoryPanel = serializedObject.FindProperty("_inventoryPanel");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Parent object of the Inventory Slots");
        serializedObject.Update();
        EditorGUILayout.PropertyField(_inventoryPanel);
        serializedObject.Update();
    }
}
