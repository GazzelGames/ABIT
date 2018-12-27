using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HudCanvas : MonoBehaviour {

    public static HudCanvas instance;
    private GameObject player;

    public EventSystem eventSystem;
    public GameObject priviousButton;

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        magicValue = new List<int>();
        player = GameObject.Find("Player");
        currentMagic = maxMagic;
        metersRunning = scaleDown = scaleUp = false;
        CreateSingleton();
        anim = GetComponent<Animator>();
        int i = 0;
        currentHP = MaxHP = 3;
        foreach(GameObject test in equippedItems)
        {
            magicValue.Add(0);
            if(test == null)
            {
                itemImages[i].enabled = false;
            }
            i++;
        }
        PlayerMangerListener.PlayerDead += PlayerDead;
    }

    private void OnApplicationQuit()
    {
       PlayerMangerListener.PlayerDead -= PlayerDead;
    }

    //these are the values of for the magik meter
    public Slider magikSlider;
    public float currentMagic;
    public int maxMagic = 100;
    public int MaxMagic{get{ return maxMagic; }set { maxMagic = value;}}
    public float CurrentMagic
    {
        get
        {
            return currentMagic;
        }
        set
        {
            currentMagic = value;
            if (!metersRunning)
            {
                metersRunning = true;
                StartCoroutine(IncreaseMagikMeter());
            }
        }
    }
    bool metersRunning;
    IEnumerator IncreaseMagikMeter()
    {
        while (currentMagic < maxMagic)
        {
            currentMagic += 1f;
            magikSlider.value = currentMagic;
            yield return null;
        }
        metersRunning = false;
        yield return null;
    }

    public Sprite[] heartSprites;
    public List<GameObject> hearts;

    //this is for all money stuff
    public Text currencyText;
    int moneyPickUp;
#pragma warning disable 649
    [SerializeField]
    private int currency;
    [SerializeField]
    private int maxCurrency;
    [SerializeField]
    private float currentHP;
#pragma warning restore 649
    public float CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {           
            currentHP = currentHP + value;
            int i = 0;
            foreach (GameObject newHeart in hearts)
            {
                if (i < currentHP)
                {
                    newHeart.GetComponent<Image>().sprite = heartSprites[0];
                }
                else
                {
                    newHeart.GetComponent<Image>().sprite = heartSprites[1];
                }
                i++;
            }
            if (currentHP <= 0)
            {
                PlayerMangerListener.instance.StateOf = GameState.StateOfGame.PlayerDead;
            }

        }
    }
    public int MaxHP { get; set; }
    public int CurrentCurrency
    {
        get
        {
            return currency;
        }
        set
        {
            moneyPickUp = value;

            if (moneyPickUp < 0)
            {
                //this needs to scale down
                if (!scaleDown)
                {
                    scaleDown = true;
                    StartCoroutine(ScaleMoneyDown());
                }                
            }
            else
            {
                // this i needs to scale up if currency is less the max
                if (currency < maxCurrency)
                {
                    if (!scaleUp&&moneyPickUp>1)
                    {
                        scaleUp = true;
                        StartCoroutine(ScaleMoneyUp());
                    }
                    else
                    {
                        currency++;
                        currencyText.text = currency.ToString();
                    }                    
                }
            }
        }
    }
    public int MaxCurrency { get { return maxCurrency;} set { maxCurrency = value; } }

    bool scaleUp,scaleDown;
    IEnumerator ScaleMoneyUp()
    {
        AudioClip moneySound = Resources.Load<AudioClip>("Audio/MoneyPickUp");
        AudioSource.PlayClipAtPoint(moneySound, player.transform.position, 1);
        int var = 0;
        int tempCurrency = currency;
        int tempMoneyPickUp = moneyPickUp;
        moneyPickUp = 0;
        while (tempMoneyPickUp > var && tempCurrency+var<maxCurrency)
        {
            var++;
            if (0 != moneyPickUp)
            {
                tempMoneyPickUp += moneyPickUp;
                moneyPickUp = 0;
            }
            currencyText.text = (tempCurrency + var).ToString();
            AudioSource.PlayClipAtPoint(moneySound, player.transform.position,1);
            //play audio clip
            yield return null;
        }
        currency = tempCurrency + var;
        scaleUp = false;
        yield return null;
    }
    IEnumerator ScaleMoneyDown()
    {
        AudioClip moneySound = Resources.Load<AudioClip>("Audio/MoneyPickUp");
        AudioSource.PlayClipAtPoint(moneySound, player.transform.position, 1);
        int var = 0;
        int tempCurrency = currency;
        int tempMoneyPickUp = moneyPickUp;
        moneyPickUp = 0;
        while (tempMoneyPickUp < var && tempCurrency + var >= 0)
        {
            var--;
            if (0 != moneyPickUp)
            {
                tempMoneyPickUp += moneyPickUp;
                moneyPickUp = 0;
            }
            currencyText.text = (tempCurrency + var).ToString();
            AudioSource.PlayClipAtPoint(moneySound, player.transform.position, 1);
            //play audio clip
            yield return null;
        }
        currency = tempCurrency + var;
        scaleDown = false;
        yield return null;
    }

    //animator reference
    Animator anim;
    public Image spriteFader;
    public void FadeInFader()
    {
        anim.SetBool("FadeInFader",true);
    }

    void PlayerDead()
    {
        player.GetComponent<Animator>().SetBool("PlayerDead", true);
        MusicManager.instance.StopMusic();
        Invoke("FadeInFader", 0.5f);
        Invoke("MovePlayerToDoor", 2f);
        player.GetComponent<Collider2D>().enabled = false;
        Invoke("FadeOutFader", 1.5f);
    }

    void MovePlayerToDoor()
    {
        player.GetComponent<Animator>().SetBool("PlayerDead", false);
        if(PlayerMangerListener.instance.LastDoor == Vector3.zero)
        {
            player.gameObject.transform.position = Vector3.zero;
        }
        else
        {
            player.gameObject.transform.position = PlayerMangerListener.instance.LastDoor;
        }
        CameraController.instance.transform.position = player.gameObject.transform.position;
        CameraController.instance.followPlayer = true;
        player.GetComponent<Collider2D>().enabled = true;
        CurrentHP = MaxHP;
        MusicManager.instance.StartMusic();
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
    }

    public void FadeOutFader()
    {
        anim.SetBool("FadeInFader", false);
    }

    //these functions are used for the operation of the Useable Items
    GameObject var;
    //this contains the gameobject to be used
    public List<GameObject> equippedItems;
    [HideInInspector]
    public List<int> magicValue;
    //shows the sprite of the current gameobject
    public List<Image> itemImages;
    private void Update()
    {
        if (var == null&&PlayerMangerListener.instance.HasControl)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) && equippedItems[0] != null&&magicValue[0]<CurrentMagic)
            {
                CreateGameObject(equippedItems[0]);
                CurrentMagic = CurrentMagic - magicValue[0];
            }else if(Input.GetKeyDown(KeyCode.Keypad2) && equippedItems[1] != null && magicValue[1] < CurrentMagic)
            {
                CreateGameObject(equippedItems[1]);
                CurrentMagic = CurrentMagic - magicValue[1];
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) && equippedItems[2] != null && magicValue[2] < CurrentMagic)
            {
                CreateGameObject(equippedItems[2]);
                CurrentMagic = CurrentMagic - magicValue[2];
            }
        }
    }

    private void LateUpdate()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(priviousButton);
        }
        else if (eventSystem.currentSelectedGameObject != priviousButton)
        {
            priviousButton = eventSystem.currentSelectedGameObject;
        }
    }

    void CreateGameObject(GameObject obj)
    {
        var = Instantiate(obj, player.transform);
    }

    public void ChangeUseableObject(GameObject obj, Sprite newSprite,int index,int newMagicCost)
    {
        equippedItems[index] = obj;
        if (newSprite == null)
        {
            itemImages[index].enabled = false;
        }else if (!itemImages[index].isActiveAndEnabled)
        {
            itemImages[index].enabled = true;
        }

        itemImages[index].sprite = newSprite;
        magicValue[index] = newMagicCost;
    }

    public void EndDemo()
    {
        SceneManager.LoadScene("MainMenu");
        //I need this to be invoked 
    }

}
