using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    public int frameRateChange = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager manager = (GameManager)target;

        if (GUILayout.Button("ChangeFrameRate"))
        {
            if (frameRateChange > 2)
            {
                frameRateChange = 0;
            }
            manager.ChangeFrameRate(frameRateChange);
            frameRateChange++;
        }
        if (GUILayout.Button("ChangeVSync"))
        {
            manager.ChangeVSync();
        }
    }
}
