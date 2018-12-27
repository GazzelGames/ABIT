//using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class Equipment : ScriptableObject {

    public int defenseBonus;
    public int attackBonus;
    public int movementBonus;
    public BasicItemValues itemValues;
}

/*public class MakeEquipmentObject
{
    [MenuItem("Assets/Create/EquipmentObject")]
    public static void Create()
    {
        Equipment asset = ScriptableObject.CreateInstance<Equipment>();
        AssetDatabase.CreateAsset(asset, "Assets/NewEquipmentAsset.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}*/