using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NecroShields))]
[CanEditMultipleObjects]
public class NecroShieldEditor : Editor{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NecroShields behavior = (NecroShields)target;
        if (GUILayout.Button("DestroyParticles"))
        {
            behavior.ResetBattle();
        }
    }
}