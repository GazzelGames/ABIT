using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuFunctionality : MonoBehaviour {

    public GameObject HUD;

    public EventSystem eventSystem;

    //AudioClips for the menu
    AudioClip buttonHighlighted;
    AudioClip buttonSelected;
    AudioClip itemEquipedSound;
    AudioSource audioSource;

    bool transitionDone = true;
    public GameObject returnButton;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttonHighlighted = Resources.Load<AudioClip>("Audio/ButtonHighlighted");
        buttonSelected = Resources.Load<AudioClip>("Audio/ButtonConfirm");
        itemEquipedSound = Resources.Load<AudioClip>("Audio/sfx_menu_select4");
    }

    private void OnEnable()
    {
        eventSystem.firstSelectedGameObject = returnButton;
        eventSystem.SetSelectedGameObject(returnButton);
        previousButton = eventSystem.currentSelectedGameObject;
        audioSource.volume = GameManager.instance.VolumeModifier;
    }

    public void ActivateUpdate()
    {
        this.enabled = true;
    }

    private GameObject previousButton;
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != previousButton)
        {
            audioSource.PlayOneShot(buttonHighlighted);
            previousButton = eventSystem.currentSelectedGameObject;
        }
    }

    new string name;
    private void LateUpdate()
    {
        if (eventSystem.currentSelectedGameObject)
        {
            GameObject tempObj = eventSystem.currentSelectedGameObject;
            name = tempObj.transform.parent.parent.name;
            if (transitionDone && (name =="ItemPanel"||name=="QuestItems"))
            {
                if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.I))
                {
                    StartCoroutine(MoveImageToButton(eventSystem.currentSelectedGameObject, 0, name));
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.O))
                {
                    StartCoroutine(MoveImageToButton(eventSystem.currentSelectedGameObject, 1,name));
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.P))
                {
                    StartCoroutine(MoveImageToButton(eventSystem.currentSelectedGameObject, 2,name));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)&& PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GamePaused&&transitionDone)
        {
            ResumeGame();
        }
    }

    //these variables will be used in the coroutine
    int magicInt;
    GameObject classItem;
    Sprite classSprite;
    IEnumerator DecidedClass(GameObject reference,string rafName)
    {
        if (rafName == "ItemPanel")
        {
            int i = PauseMenuManager.instance.pauseManger.actionItemClass.GetButtonIndex(reference);
            ActionItem actionItem = PauseMenuManager.instance.pauseManger.actionItemClass.GetItemFromIndex(i);
            classSprite = actionItem.itemValues.itemImage;
            classItem = actionItem.aiObject;
            magicInt = actionItem.magicCost;
        }
        else
        {
            int i = PauseMenuManager.instance.pauseManger.questItemClass.GetButtonIndex(reference);
            QuestItem questItem = PauseMenuManager.instance.pauseManger.questItemClass.GetItemFromIndex(i);
            classSprite = questItem.itemValues.itemImage;
            classItem = questItem.questItem;
            magicInt = 0;
        }
        yield return null;
    }
    IEnumerator MoveImageToButton(GameObject temp, int newElement,string refName)
    {
        audioSource.PlayOneShot(itemEquipedSound);
        transitionDone = false;                                 
        yield return StartCoroutine(DecidedClass(temp, refName));     
        CheckCurrentItem(newElement);                     
        GameObject tempObj = Instantiate(temp, HUD.transform);  
        Destroy(tempObj.GetComponent<Button>());               
        tempObj.transform.position = temp.transform.position;   

        HudCanvas.instance.itemImages[newElement].enabled=false;       
        Vector3 distance = HudCanvas.instance.itemImages[newElement].transform.position - temp.transform.position;
        float totalLenght = distance.magnitude;
        float f;
        do
        {
            tempObj.transform.position = Vector3.Lerp(tempObj.transform.position, HudCanvas.instance.itemImages[newElement].transform.position, 0.1f);
            distance = HudCanvas.instance.itemImages[newElement].transform.position - tempObj.transform.position;
            f = distance.magnitude;
            if (totalLenght / 10 < f)
            {
                tempObj.transform.localScale += new Vector3(3.5f * Time.deltaTime, 3.5f * Time.deltaTime, 0);
            }
            else
            {
                tempObj.transform.localScale -= new Vector3(3.5f * Time.deltaTime, 3.5f * Time.deltaTime, 0);
            }
            yield return new WaitForSeconds(0.01f);
        } while (f > 0.1f);
        HudCanvas.instance.ChangeUseableObject(classItem, classSprite, newElement,magicInt);
        HudCanvas.instance.itemImages[newElement].enabled = true;
        Destroy(tempObj);

        transitionDone = true;
        yield return null;
    }

    //this class needs to use classSprite and classItem
    void CheckCurrentItem(int index)
    {
        int swapPosition = 0;
        foreach (GameObject @object in HudCanvas.instance.equippedItems)
        {
            if (classItem == @object)
            {
                if (swapPosition != index)
                { 
                    HudCanvas.instance.ChangeUseableObject(HudCanvas.instance.equippedItems[index], HudCanvas.instance.itemImages[index].sprite, swapPosition,HudCanvas.instance.magicValue[index]);
                    break;
                }
            }
            swapPosition++;          
        }
    }

    //this is when you want to resume the game
    public void ResumeGame()
    {
        audioSource.PlayOneShot(buttonSelected);
        this.enabled = false;
        GetComponent<Animator>().SetTrigger("TurnOff");
        //PlayerMangerListener.instance.IsPaused = false;
    }
    public void SetDisabled()
    {
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
        gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(buttonSelected);
        this.enabled = false;
        HudCanvas.instance.FadeInFader();
        Invoke("LoadMainMenu", 1.2f);

        /*
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.QuitGame();
        #endif
        /*
        #if UNITY_STANDALONE_WIN
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.QuitGame();
        #endif*/
    }

    void LoadMainMenu()
    {
        #if UNITY_STANDALONE_WIN
                Application.Quit();
        #endif

        SceneManager.LoadScene("MainMenu");
    }
}
