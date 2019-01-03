using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenuManager : MonoBehaviour {

    //this is for creating a signleton reference for this class
    public static PauseMenuManager instance;
    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //this is used for creating the data management classes
    public PauseMangerClass pauseManger;
    public GameObject actionParent;
    public GameObject equipmentParent;
    public GameObject questParent;


    public List<GameObject> Aobjs;
    public List<GameObject> Eobjs;
    public List<GameObject> Qobjs;
    //this initializes the data structures for use
    void CreateDataStructures()
    {
        pauseManger.actionItemClass.CreateData(actionParent.transform.GetChild(0).gameObject, actionParent.transform.GetChild(1).gameObject);
        Aobjs = pauseManger.actionItemClass.ReturnList();
        pauseManger.equipmentItemClass.CreateData(equipmentParent.transform.GetChild(0).gameObject, null);
        //Eobjs = pauseManger.equipmentItemClass.ReturnList();
        pauseManger.questItemClass.CreateData(questParent.transform.GetChild(0).gameObject, questParent.transform.GetChild(1).gameObject);
        Qobjs = pauseManger.questItemClass.ReturnList();
    }

    //create quest item in the pause menu
    public void CreateQuestItem(QuestItem questItem)
    {
        int i = pauseManger.questItemClass.AddQuestItem(questItem);    
        GameObject reference = pauseManger.questItemClass.GetButtonFromIndex(i);
        reference.GetComponent<Image>().sprite = questItem.itemValues.itemImage;
        ChangeButtonState(true, reference);
    }
    //once the quest is ended there will be no reason to keep it in the pause menu
    public void EndQuestItem(QuestItem item)
    {
        int i = pauseManger.questItemClass.GetItemIndex(item);
        pauseManger.questItemClass.RemovingItem(i);
        GameObject reference = pauseManger.questItemClass.GetButtonFromIndex(i);
        reference.GetComponent<Image>().sprite = null;
        ChangeButtonState(false, reference);

        int loopCount = 0;
        foreach(GameObject obj in HudCanvas.instance.equippedItems)
        {
            if(obj == item.questItem)
            {
                HudCanvas.instance.ChangeUseableObject(null,null,loopCount,0);
                break;
            }
            loopCount++;
        }
    }

    //this will be for adding action items, that way we have tools in our inventory to use
    public void AddActionItem(int index, ActionItem actionItem)
    {
        pauseManger.actionItemClass.AssingItem(actionItem, index);
        GameObject reference = pauseManger.actionItemClass.GetButtonFromIndex(index);
        reference.GetComponent<Image>().sprite = actionItem.itemValues.itemImage;
        ChangeButtonState(true, reference);
    }

    //this is for changing equipment as it is upgraded
    public void ChangeEquipmentItem(int index, Equipment equipment)
    {
        pauseManger.equipmentItemClass.AssingItem(equipment,index);
        GameObject reference = pauseManger.equipmentItemClass.GetButtonFromIndex(index);
        reference.GetComponent<Image>().sprite = equipment.itemValues.itemImage;
        ChangeButtonState(true, reference);
    }

    //this will return the equipment at an index to check all the things
    public Equipment CheckEquipmentItem(int index)
    {
        return pauseManger.equipmentItemClass.GetItemFromIndex(index);
    }

    //this returns the damage value for the sword 
    public int SwordDamage()
    {
        if (pauseManger.equipmentItemClass.GetItemFromIndex(2) != null)
        {
            return pauseManger.equipmentItemClass.GetItemFromIndex(2).attackBonus;
        }
        else
        {
            return 0;
        }

    }
    //this returns the defensive value of the armor
    public int ArmorValue()
    {
        return pauseManger.equipmentItemClass.GetItemFromIndex(1).defenseBonus;
    }


    //this is for changing button state, simplifing the changing the button state
    //and it is only to be used within this class
    private void ChangeButtonState(bool var, GameObject buttonReference)
    {
        Button button = buttonReference.GetComponent<Button>();
        Image image = buttonReference.GetComponent<Image>();
        image.enabled = var;
        button.enabled = var;
    }

    public float Sword;

    private void Awake()
    {
        pauseManger = new PauseMangerClass();
        CreateSingleton();
        CreateDataStructures();
        Sword = SwordDamage();
    }

    //this is for testing it will be removed later
    public Equipment wallet;
    public Equipment sword;
    public QuestItem hammer;
    public ActionItem fireBall;
    public ActionItem iceSpell;
    public ActionItem shield;
    //I need to test to see if the objects are going in and out of the data structure
    
    public void AddItem(int i)
    {
        if (i == 0)
        {
            AddActionItem(0, fireBall);
        }else if (i == 1)
        {
            AddActionItem(1, iceSpell);
        }
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateQuestItem(hammer);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeEquipmentItem(3, wallet);
        }else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddActionItem(0, fireBall);
        }else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddActionItem(1, iceSpell);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddActionItem(2, shield);
        }else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeEquipmentItem(2, sword);
        }
    }*/
}