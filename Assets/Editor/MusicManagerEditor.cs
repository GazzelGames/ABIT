using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MusicManager))]
[CanEditMultipleObjects]
public class MusicManagerEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MusicManager behavior = (MusicManager)target;
    }
}