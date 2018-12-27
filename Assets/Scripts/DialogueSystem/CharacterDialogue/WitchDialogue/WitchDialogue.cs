using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDialogue : MonoBehaviour {

    public NormalDialogue intro;
    public NormalDialogue waiting;
    public NormalDialogue questDone;
    public GameObject shieldSpell;

    WitchDialogueListener witchDialogueListener;

    [HideInInspector]
    public bool waitingForMushroom;
    [HideInInspector]
    public bool mushRoomRecieved;
    bool canSubscribe;

    private void Awake()
    {
        canSubscribe = true;
        witchDialogueListener = new WitchDialogueListener();
        witchDialogueListener.SetReferences(GetComponent<WitchDialogue>(), GetComponent<BoxCollider2D>());
    }
    private void OnEnable()
    {
        if (canSubscribe)
        {
            witchDialogueListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            witchDialogueListener.Unsubscribe();
            canSubscribe = true;
        }
    }

    private void Start()
    {
        waitingForMushroom = mushRoomRecieved = false;
    }

    private void GetDialogue()
    {
        if (mushRoomRecieved)
        {
            DialogueManager.instance.StartTalking(questDone);
        }else if (waitingForMushroom)
        {
            DialogueManager.instance.StartTalking(waiting);
        }
        else
        {
            DialogueManager.instance.StartTalking(intro);
            waitingForMushroom = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.enabled = true;
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.enabled = false;
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            PlayerDialogueTransmitter.instance.objectReference = null;
        }
        DialogueManager.instance.ClearText();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == gameObject)
        {
            DialogueManager.instance.DisplayTalk();
            if (Input.GetKey(KeyCode.C) && PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening)
            {
                GetDialogue();
                DialogueManager.instance.ClearText();
            }
        }
    }
}

class WitchDialogueListener
{
    WitchDialogue witchDialogue;
    BoxCollider2D collider2D;

    public void SetReferences(WitchDialogue witchDialogue, BoxCollider2D collider2D)
    {
        this.witchDialogue = witchDialogue;
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
        witchDialogue.enabled = false;
        collider2D.enabled = false;
    }

    public void GameResumed()
    {
        witchDialogue.enabled = true;
        collider2D.enabled = true;
    }

}

