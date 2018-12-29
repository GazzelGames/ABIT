using UnityEngine;

[System.Serializable]
public class QuestItem : ScriptableObject{

    private GameObject pauseMenuButton;  //this is the button in the pause menu that
    private GameObject actionItemReference;
    public GameObject questItem;
    public BasicItemValues itemValues;
}
