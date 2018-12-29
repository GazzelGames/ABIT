using UnityEditor;
using UnityEngine;

public class CreatingScriptableObjects : ScriptableObject {

    [MenuItem("Assets/Create/ActionItemObject")]
    public static void CreateAI()
    {
        ActionItem asset = CreateInstance<ActionItem>();
        AssetDatabase.CreateAsset(asset, "Assets/NewActionItemAsset.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/QuestItemObject")]
    public static void CreateQ()
    {
        QuestItem asset = ScriptableObject.CreateInstance<QuestItem>();
        AssetDatabase.CreateAsset(asset, "Assets/NewQuestItem.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/EquipmentObject")]
    public static void CreateE()
    {
        Equipment asset = ScriptableObject.CreateInstance<Equipment>();
        AssetDatabase.CreateAsset(asset, "Assets/NewEquipmentAsset.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}