using UnityEngine;
public class GenericDialogue : MonoBehaviour {

    public int element;
    public NormalDialogue[] dialogue;
    GenericDialogueListener genericDialogueListener;
    bool canSubscribe;

    private void Awake()
    {
        genericDialogueListener = new GenericDialogueListener();
        genericDialogueListener.SetReferences(GetComponent<GenericDialogue>(),GetComponent<BoxCollider2D>());
        canSubscribe = true;
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            genericDialogueListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            genericDialogueListener.Unsubscribe();
            canSubscribe = true;
        }

    }

    public void Start()
    {
        anim = GetComponentInParent<Animator>();
        element = 0;
    }

    void GetDialogue()
    {
        if (element >= dialogue.Length)
        {
            element = 0;
        }
        DialogueManager.instance.StartTalking(dialogue[element]);
        element++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetFloat("IdleStance", 0);
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            GetComponent<GenericDialogue>().enabled = true;
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        GetComponent<GenericDialogue>().enabled = false;
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            PlayerDialogueTransmitter.instance.objectReference = null;
            DialogueManager.instance.ClearText();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            DialogueManager.instance.DisplayTalk();
            if (Input.GetKey(KeyCode.C) && PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening)
            {
                GetDialogue();
                SetTalkingStance();
                DialogueManager.instance.ClearText();
            }
        }

    }

    Animator anim;
    private void SetTalkingStance()
    {
        anim.SetBool("IsMoving", false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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

class GenericDialogueListener
{
    GenericDialogue genericDialogue;
    BoxCollider2D collider2D;

    public void SetReferences(GenericDialogue genericDialogue, BoxCollider2D collider2D)
    {
        this.genericDialogue = genericDialogue;
        this.collider2D = collider2D;
    }

    //I need a method of doing this 
    public void Subscribe()
    {
        PlayerMangerListener.GameStopped+= GameStopped;
        PlayerMangerListener.GameResumed += GameResumed;
    }

    public void Unsubscribe()
    {
        PlayerMangerListener.GameStopped -= GameStopped;
        PlayerMangerListener.GameResumed -= GameResumed;
    }

    public void GameStopped()
    {
        genericDialogue.enabled = false;
        collider2D.enabled = false;
    }

    public void GameResumed()
    {               
        genericDialogue.enabled = true;
        collider2D.enabled = true;
    }
        
}
