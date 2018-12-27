using System.Collections;
using UnityEngine;
using UnityEditor;
[System.Serializable]
public class QuestItem : ScriptableObject{

    private GameObject pauseMenuButton;  //this is the button in the pause menu that
    private GameObject actionItemReference;
    public GameObject questItem;
    public BasicItemValues itemValues;
}

/*public class MakeQuestObject
{
    [MenuItem("Assets/Create/QuestItemObject")]
    public static void Create()
    {
        QuestItem asset = ScriptableObject.CreateInstance<QuestItem>();
        AssetDatabase.CreateAsset(asset, "Assets/NewQuestItem.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}*/