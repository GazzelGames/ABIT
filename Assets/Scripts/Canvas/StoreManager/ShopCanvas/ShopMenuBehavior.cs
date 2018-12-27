using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopMenuBehavior: MonoBehaviour {

    //public reference to the event system
    private EventSystem eventSystem;

    //this is the reference to the UI Buttons
    public List<GameObject> getButtons;
    public List<ActionItem> actionItems;
    int[] menuLocation = new int[3];
    AudioClip buttonHighlighted;
    AudioClip buttonConfirmed;
    AudioClip displayText;
    AudioSource audioSource;

    public QuestItem questItem;

    //this dictionary contains the info that the buttons will need to use;
    [HideInInspector]
    public Dictionary<GameObject, StoreButtonClass> keyValuePairs;

    //these are the references to the UI Objecst
    public Text discription;
    public Text moneyValue;
    public Image itemImage;
    public Animator lucasAnim;
    //GameObject lucasShopKeeper;
    //SpriteRenderer lucasSprite;

    [HideInInspector]
    [SerializeField]
    private List<GameObject> decideAction; //these are the options for when you first enter the shop

    private void CreateDictionary()
    {
        menuLocation[0] = 6;
        menuLocation[1] = 5;
        menuLocation[2] = 7;
        int i = 0;
        foreach(GameObject obj in getButtons)
        {
            StoreButtonClass storeButtonClass = new StoreButtonClass();
            if (i < 3)
            {
                storeButtonClass.image = actionItems[i].itemValues.itemImage;
                storeButtonClass.discription = actionItems[i].itemValues.discription;
                storeButtonClass.moneyValue = actionItems[i].itemValues.moneyValue.ToString();
                storeButtonClass.itemName = actionItems[i].itemValues.itemName;
                storeButtonClass.quintonValue = actionItems[i].itemValues.moneyValue;
                storeButtonClass.index = menuLocation[i];
                storeButtonClass.isAction = true;
            }
            else
            {
                storeButtonClass.image = questItem.itemValues.itemImage;
                storeButtonClass.discription = questItem.itemValues.discription;
                storeButtonClass.moneyValue = questItem.itemValues.moneyValue.ToString();
                storeButtonClass.itemName = questItem.itemValues.itemName;
                storeButtonClass.quintonValue = questItem.itemValues.moneyValue;
                storeButtonClass.isQuest = true;
            }
            keyValuePairs.Add(obj,storeButtonClass);
            i++;

            buttonHighlighted = Resources.Load<AudioClip>("Audio/ButtonHighlighted");
            buttonConfirmed = Resources.Load<AudioClip>("Audio/ButtonConfirmed");
            displayText = Resources.Load<AudioClip>("Audio/TextSound");
            priviousButton = eventSystem.currentSelectedGameObject;

            //print("declare dictionary");
        }
    }
    private void InitializeButtons()
    {
        foreach(GameObject obj in getButtons)
        {
            Text text = obj.transform.GetChild(0).gameObject.GetComponent<Text>();
            text.text = keyValuePairs[obj].itemName;
        }

        decideAction.Add(transform.GetChild(0).GetChild(2).GetChild(0).gameObject);
        decideAction.Add(transform.GetChild(0).GetChild(2).GetChild(1).gameObject);
        decideAction.Add(transform.GetChild(0).GetChild(2).GetChild(2).gameObject);
    }

    //these are references to enable them when you want to start shopping
    public GameObject itemDiscriptionPanel;
    public GameObject listOfItems;

    private void Awake()
    {
        confirmPurchaseButtons = transform.GetChild(2).gameObject;
        eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
        keyValuePairs = new Dictionary<GameObject, StoreButtonClass>();
        audioSource = GetComponent<AudioSource>();
        Invoke("CreateDictionaries",0);
    }
    void CreateDictionaries()
    {
        CreateDictionary();
        InitializeButtons();
    }

    private void OnEnable()
    {
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.PlayerShopping;
        audioSource.volume = GameManager.instance.VolumeModifier;
        element = 0;
        statmentRunning = false;
        CameraController.instance.followPlayer = false;
        CameraController.instance.targetReached = false;
        Invoke("AssignButton", 0.05f);
    }
    void AssignButton()
    {
        eventSystem.firstSelectedGameObject = transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        eventSystem.SetSelectedGameObject(transform.GetChild(0).GetChild(2).GetChild(0).gameObject);
        if (speechBox.activeInHierarchy)
        {
            itemDiscriptionPanel.SetActive(false);
            speechBox.SetActive(false);
        }
    }

    //when game object is disabled
    private void OnDisable()
    {
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
        //PlayerMangerListener.instance.GameHaulted = false;
        eventSystem.firstSelectedGameObject = null;
        eventSystem.SetSelectedGameObject(null);
        lucasAnim.enabled = true;
        CameraController.instance.followPlayer = true;
    }

    //when you select the start shoping button
    public void StartShopping()
    {
        PlayAudioClip(buttonConfirmed);
        listOfItems.SetActive(true);
        itemDiscriptionPanel.SetActive(true);
        speechBox.SetActive(false);
        eventSystem.firstSelectedGameObject = listOfItems.transform.GetChild(0).gameObject;
        eventSystem.SetSelectedGameObject(listOfItems.transform.GetChild(0).gameObject);
    }

    //this variable is used to start and stop the item discription coroutine and for changing active button
    Coroutine itemDisciption;
    public void OnButtonEnter() //when you move on this button this stuff happens
    {
        //PlayAudioClip(buttonHighlighted);
        GameObject currentSelected = eventSystem.currentSelectedGameObject.gameObject;
        StoreButtonClass @class = keyValuePairs[currentSelected]; //this dictionary returns the storebutton class
        itemDisciption = StartCoroutine(ScrollDiscription(@class.discription));
        itemImage.sprite = @class.image;
        moneyValue.text = @class.moneyValue;
    }
    //when you move off of the button 
    public void OnButtonExit()
    {
        if (itemDisciption != null)
        {
            StopCoroutine(itemDisciption); //this stops the scrolling of the text for the privious item
        }
        discription.text = "";
        moneyValue.text = "";
        itemImage.sprite = null;
    }

    //this used to scroll the discriptions
    public IEnumerator ScrollDiscription(string newDiscription)
    {
        bool test=true;
        foreach (char letter in newDiscription)
        {
            discription.GetComponent<Text>().text += letter;
            if (test)
            {
                test = false;
            }
            else
            {
                test = true;
            }

            yield return null;
        }
        yield return null;
    }

    public GameObject confirmPurchaseButtons;  //this turns on the Yes No Buttons
    private GameObject itemInQuestion;      //this get the button as the key value pair for the dictionary
    private StoreButtonClass itemSelected;  //this is the item associated in store button class
    public void OnPurchase()
    {
        PlayAudioClip(buttonConfirmed);
        string stringText;
        itemInQuestion = eventSystem.currentSelectedGameObject;
        itemSelected = keyValuePairs[itemInQuestion];
        listOfItems.SetActive(false);
        if (HudCanvas.instance.CurrentCurrency >= itemSelected.quintonValue)
        {

            confirmPurchaseButtons.SetActive(true);

            eventSystem.SetSelectedGameObject(confirmPurchaseButtons.transform.GetChild(1).gameObject);     //this sets the no option as the current game object
            stringText = "Are you sure that you would like to by: " + itemSelected.itemName + "?";
            StartCoroutine(AreYouBuying(stringText,false));
        }
        else
        {
            stringText = "Im sorry you can't afford it right now, otherwise I would sell it to you";
            StartCoroutine(AreYouBuying(stringText, true));
        }
        speechBox.SetActive(true);

    }

    IEnumerator AreYouBuying(string statment, bool bought)
    {
        statmentRunning = true;
        text.text = "";
        foreach (char letter in statment)
        {
            PlayAudioClip(displayText);
            text.text += letter;
            yield return null;
        }
        if (bought)
        {
            Invoke("AssignButton", 2);
        }
        statmentRunning = false;        
        yield return null;
    }

    //when you click the BuyItem Button
    void BuyItem()
    {
        if (itemSelected.isAction)
        {
            foreach (ActionItem action in actionItems)
            {
                if (action.itemValues.itemName == itemSelected.itemName)
                {
                    PauseMenuManager.instance.AddActionItem(itemSelected.index, action);
                    break;
                }
            }
        }
        else
        {
            PauseMenuManager.instance.CreateQuestItem(questItem);
        }
        HudCanvas.instance.CurrentCurrency = -itemSelected.quintonValue;
        keyValuePairs.Remove(itemInQuestion);
        Destroy(itemInQuestion);
        speechBox.SetActive(true);
        confirmPurchaseButtons.SetActive(false);
        StartCoroutine(AreYouBuying("Oh Thank you so much for you purchase!", true));
        itemSelected = null;
        itemInQuestion = null;
    }

    GameObject priviousButton;
    private void Update()
    {
        if(eventSystem.currentSelectedGameObject != priviousButton)
        {
            PlayAudioClip(buttonHighlighted);
            priviousButton = eventSystem.currentSelectedGameObject;
        }
    }

    //click yes to Buy Item
    public void Yes()
    {
        //this is where the purchasing code goes
        eventSystem.SetSelectedGameObject(listOfItems.transform.GetChild(0).gameObject);    //this is the first gameobject to the first button in the list
        PlayAudioClip(buttonConfirmed);
        BuyItem();
    }

    //Click no to not buy item
    public void No()
    {
        PlayAudioClip(buttonConfirmed);
        StartCoroutine(AreYouBuying("Oh, well no problem we have other things on sale.", true));
        confirmPurchaseButtons.SetActive(false);    //this disables the 
        eventSystem.SetSelectedGameObject(listOfItems.transform.GetChild(0).gameObject);    //this is the first gameobject to the first button in the list
    }

    //these variables are used for the in shop Keeper dialogue
    int element;
    [TextArea(3, 10)]
    public string[] talkDialogue;
    public Text text;
    public GameObject speechBox;
    bool statmentRunning;
    //these are the methods for the talk button
    public void Talk()
    {
        PlayAudioClip(buttonConfirmed);
        listOfItems.SetActive(false);
        itemDiscriptionPanel.SetActive(false);
        if (element >= talkDialogue.Length)
        {
            element = 0;
        }

        if (!statmentRunning)
        {
            speechBox.SetActive(true);
            statmentRunning = true;
            StartCoroutine(SimpleStatment(talkDialogue[element]));
            element++;
        }
    }

    //this is for scrolling a simple statment
    IEnumerator SimpleStatment(string statment)
    {
        GameObject previousSelected = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(null);

        text.text = "";
        foreach (char letter in statment)
        {
            text.text += letter;
            PlayAudioClip(displayText);
            yield return null;
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
        speechBox.SetActive(false);
        statmentRunning = false;
        eventSystem.SetSelectedGameObject(previousSelected);
        yield return null;
    }

    //this method is for leaving the store
    public void Exit()
    {
        PlayAudioClip(buttonConfirmed);
        gameObject.SetActive(false);
        itemDiscriptionPanel.SetActive(false);
        listOfItems.SetActive(false);
        StopAllCoroutines();
    }

    void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
