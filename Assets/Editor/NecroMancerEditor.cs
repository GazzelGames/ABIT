using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NecroMancerBehavior))]
[CanEditMultipleObjects]
public class NecroMancerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NecroMancerBehavior behavior = (NecroMancerBehavior)target;
        if (GUILayout.Button("SpawnZombies"))
        {
            behavior.SpawnZombies();
        }
        if (GUILayout.Button("SpawnShield"))
        {
            behavior.StartCastingShield();
        }
    }
}
