using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager instance;

    Text displayInsruction;
    string pressToTalk = "Press C to talk.";
    string pressToOpen = "Press C to open.";
    string pressToRead = "Press C to read sign.";

    AudioSource displayCharacter;

    public void ReadSign()
    {
        displayInsruction.text = pressToRead;
    }
    public void DisplayTalk()
    {
        displayInsruction.text = pressToTalk;
    }
    public void DisplayToOpen()
    {
        displayInsruction.text = pressToOpen;
    }
    public void ClearText()
    {
        displayInsruction.text = "";
    }


    public Text text;
    Vector3 startTextPos;
    public GameObject speechBox;
    Animator anim;
    int length = 0;
    [HideInInspector]
    public int currentIndex = 0;
    int letterIndex = 0;
    bool latchkey = true;
    bool everyOtherFrame = false;

    public void CreateSingleton()
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
        displayInsruction = gameObject.transform.GetChild(5).gameObject.GetComponent<Text>();
        displayCharacter = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        length = 0;
        currentIndex = 0;
        letterIndex = 0;
        CreateSingleton();
    }

    bool scrollText;
    public char[] letters;
    public string[] allDialogue;
    public void StartTalking(NormalDialogue dialogue)
    {
        displayCharacter.volume = GameManager.instance.VolumeModifier;
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.PlayerTalking;
        //PlayerMangerListener.instance.Talking = true;
        allDialogue = new string[dialogue.dialogue.Length];
        length = dialogue.dialogue.Length;
        allDialogue = dialogue.dialogue;
        speechBox.SetActive(true);
        instance.enabled = true;
    }

    private void LateUpdate()
    {
        //meaning that if the text isnt scrolling then 
        if (!scrollText)
        {
            if (currentIndex < length)
            {
                if (latchkey)
                {
                    //this makes letters equal to a character array
                    letterIndex = 0;
                    letters = new char[allDialogue[currentIndex].Length];
                    letters = allDialogue[currentIndex].ToCharArray();
                    latchkey = false;
                    text.text = "";
                }
                else if (letterIndex < letters.Length)
                {
                    if (everyOtherFrame||Input.GetKey(KeyCode.C))
                    {
                        try
                        {
                            text.text += letters[letterIndex];
                            text.text += letters[letterIndex+1];
                            letterIndex += 2;
                            displayCharacter.PlayOneShot(displayCharacter.clip);
                            everyOtherFrame = false;
                        }
                        catch
                        {
                            text.text += letters[letterIndex];
                            letterIndex++;
                            displayCharacter.PlayOneShot(displayCharacter.clip);
                            everyOtherFrame = false;
                        }

                    }
                    else
                    {
                        everyOtherFrame = true;
                    }

                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    anim.SetTrigger("ScrollText");
                    scrollText = true;
                    latchkey = true;
                    currentIndex++;
                }
            }
            else    // this is us
            {
                text.text = "";
                latchkey = true;
                length = currentIndex = letterIndex = 0;
                if (!PlayerMangerListener.instance.InScene)
                {
                    print("returning to the gameLIstening state");
                    speechBox.SetActive(false);
                    PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
                    //PlayerMangerListener.instance.Talking = false;
                }
                instance.enabled = false;
            }
        }
    }

    void NextSentence()
    {
        if (currentIndex < length)
        {
            letterIndex = 0;
            letters = new char[allDialogue[currentIndex].Length];
            letters = allDialogue[currentIndex].ToCharArray();
            latchkey = false;
            text.text = "";
        }
        scrollText = false;

    }
}
