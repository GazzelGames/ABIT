using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSpriteFlickering))]
[CanEditMultipleObjects]
public class PlayerSpriteFlickerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerSpriteFlickering behavior = (PlayerSpriteFlickering)target;

        if (GUILayout.Button("BeginFlicker"))
        {
            behavior.Begin();
        }
    }
}
