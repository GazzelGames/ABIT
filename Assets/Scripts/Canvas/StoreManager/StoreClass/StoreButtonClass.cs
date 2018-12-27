using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class StoreButtonClass
{
    public void AssignBool(int itemBool)
    {
        switch (itemBool)
        {
            case 0:
            {
                isAction = true;
                break;
            }
            case 1:
            {
                isEquipment = true;
                break;
            }
            case 2:
            {
                isQuest = true;
                break;
            }
        }
    }
    public Sprite image;
    [TextArea(3, 10)]
    public string discription;
    public string moneyValue;
    public string itemName;

    public int quintonValue;
    public int index;
    //these values will get assigned to the 
    public bool isAction = false;
    public bool isEquipment = false;
    public bool isQuest = false;
   
}
