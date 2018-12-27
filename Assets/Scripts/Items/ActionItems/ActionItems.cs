using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class ActionItem : ScriptableObject {

    //Equipment Stats stuff. I am trying to figure it out
    public int damage;
    public int magicCost;
    public GameObject aiObject;
    public BasicItemValues itemValues;

}

/*public class MakeActionItemObject
{
    [MenuItem("Assets/Create/ActionItemObject")]
    public static void Create()
    {
        ActionItem asset = ScriptableObject.CreateInstance<ActionItem>();
        AssetDatabase.CreateAsset(asset, "Assets/NewActionItemAsset.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}*/
