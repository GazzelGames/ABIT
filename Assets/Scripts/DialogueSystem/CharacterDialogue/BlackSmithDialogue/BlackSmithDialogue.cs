using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackSmithDialogue : MonoBehaviour {

    BlackSmithDialogueListener dialogueListener;
    bool canSubscribe;

    public bool intro;
    public GameObject wallet;
    public NormalDialogue introDialogue;
    public bool dialogueLatchKey;

    private GameObject player;
    public bool waitingForHammer;
    public NormalDialogue waitForHammerDialogue;

    public bool hammerRecieved;
    public NormalDialogue nd;

    private void Awake()
    {
        canSubscribe = true;
        dialogueListener = new BlackSmithDialogueListener();
        dialogueListener.SetReferences(GetComponent<BlackSmithDialogue>(), GetComponent<BoxCollider2D>());
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            dialogueListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            dialogueListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    public void Start()
    {
        anim = GetComponentInParent<Animator>();
        player = GameObject.Find("Player");
        intro = true;
        waitingForHammer = hammerRecieved = false;
    }

    NormalDialogue stuff;
    private void GetDialogue()
    {
        SetTalkingStance();
        if (intro)
        {
            StartCoroutine(IntroWallet());
            intro = false;
            waitingForHammer = true;
        }else if(waitingForHammer){
            
            DialogueManager.instance.StartTalking(waitForHammerDialogue);
        }else if(hammerRecieved){
            DialogueManager.instance.StartTalking(nd);
        }
    }

    public GameObject purpleQuinton;
    IEnumerator IntroWallet()
    {
        PlayerMangerListener.instance.InScene = true;
        DialogueManager.instance.StartTalking(introDialogue);
        yield return new WaitUntil(()=>DialogueManager.instance.enabled==false);


        //PlayerMangerListener.instance.GameHaulted = true;
        yield return new WaitForSeconds(0.25f);
        GameObject @object = Instantiate(wallet, player.transform);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        yield return new WaitUntil(() => @object == null);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        Instantiate(purpleQuinton, player.transform.position,player.transform.rotation);

        //PlayerMangerListener.instance.GameHaulted = false;

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<BlackSmithDialogue>().enabled = true;
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        GetComponent<BlackSmithDialogue>().enabled = false;
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            PlayerDialogueTransmitter.instance.objectReference = null;
        }
        DialogueManager.instance.ClearText();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            DialogueManager.instance.DisplayTalk();
            if (Input.GetKey(KeyCode.C) && PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening)
            {
                GetDialogue();
                DialogueManager.instance.ClearText();
            }
        }
    }

    Animator anim;
    private void SetTalkingStance()
    {
        Vector3 displacement = gameObject.transform.parent.position - player.transform.position;
        float x = displacement.x;
        float y = displacement.y;
        float stance = 0;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
            {
                stance = 1;
            }
            else
            {
                stance = 2;
            }
        }
        else
        {
            if (y > 0)
            {
                stance = 0;
            }
            else
            {
                stance = 3;
            }
        }

        anim.SetFloat("IdleStance", stance);
    }
}

class BlackSmithDialogueListener
{
    BlackSmithDialogue blackSmithDialogue;
    BoxCollider2D collider2D;

    public void SetReferences(BlackSmithDialogue blackSmithDialogue, BoxCollider2D collider2D)
    {
        this.blackSmithDialogue = blackSmithDialogue;
        this.collider2D = collider2D;
    }

    //I need a method of doing this 
    public void Subscribe()
    {
        PlayerMangerListener.GameStopped += GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;
    }

    public void Unsubscribe()
    {
        PlayerMangerListener.GameStopped -= GameStopped;
        PlayerMangerListener.GameResumed -= GameResumed;
    }

    public void GameStopped()
    {
        blackSmithDialogue.enabled = false;
        collider2D.enabled = false;
    }

    public void GameResumed()
    {
        blackSmithDialogue.enabled = true;
        collider2D.enabled = true;
    }

}
