using UnityEditor;
using UnityEngine;

/*
[CustomEditor(typeof(OverWorldController))]
[CanEditMultipleObjects]
public class EditOverWorldController : Editor
{
    private SerializedProperty npcList;
    private SerializedProperty currentCollider;
    private void OnEnable()
    {
        npcList = serializedObject.FindProperty("npcs");
        currentCollider = serializedObject.FindProperty("boxCollider2d");
    }
    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.PropertyField(currentCollider, new GUIContent("The Npcs Trigger"));

        EditorGUILayout.PropertyField(npcList, new GUIContent("List of NPC's"));

        serializedObject.ApplyModifiedProperties();
    }
}*/
