using System.Collections.Generic;
using UnityEngine;

public class PauseMangerClass {

    public class PauseOrganizationClass<T>
    {
        public List<GameObject> buttonRefs;
        List<T> items;
        public T itemData;

        public void CreateData(GameObject parent1, GameObject parent2)
        {
            buttonRefs = new List<GameObject>();
            items = new List<T>();
            itemData = default(T);

            if (parent1 != null)
            {
                LoopThroughData(parent1);
            }
            if (parent2 != null)
            {
                LoopThroughData(parent2);
            }
        }
        //this while loop is used to declare the two lists but is named poorly
        void LoopThroughData(GameObject gameObject)
        {
            int numberOfButtons = gameObject.transform.childCount;
            int i = 0;
            while (i < numberOfButtons)
            {
                buttonRefs.Add(gameObject.transform.GetChild(i).gameObject);
                items.Add(itemData);
                i++;
            }
        }

        //this gets the generic Item Data
        public T GetItemData(GameObject gameObject)
        {
            int index = buttonRefs.IndexOf(gameObject);
            return items[index];
        }

        public int GetButtonIndex(GameObject button)
        {
            return buttonRefs.IndexOf(button);
        }
        public GameObject GetButtonFromIndex(int index)
        {
            return buttonRefs[index];
        }

        public int GetItemIndex(T generic)
        {
            return items.IndexOf(generic);
        }
        public T GetItemFromIndex(int index)
        {
            return items[index];
        }

        public void AssingItem(T generic, int index)
        {
            items[index] = generic;
        }

        public void RemovingItem(int index)
        {
            items[index] = default(T);
        }

        public int AddQuestItem(T questItem)
        {
            int i = 0;
            foreach(T item in items)
            {
                if (EqualityComparer<T>.Default.Equals(item, default(T)))
                {
                    i = items.IndexOf(item);
                    items[i] = questItem;
                    break;
                }
            }
            return i;
        }

        public List<GameObject> ReturnList()
        {
            return buttonRefs;
        }
        
    }

    public PauseOrganizationClass<ActionItem> actionItemClass = new PauseOrganizationClass<ActionItem>();
    public PauseOrganizationClass<Equipment> equipmentItemClass = new PauseOrganizationClass<Equipment>();
    public PauseOrganizationClass<QuestItem> questItemClass = new PauseOrganizationClass<QuestItem>();

}
