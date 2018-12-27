using UnityEngine;
using UnityEngine.UI;

public class InanimateDialogue : MonoBehaviour {

    public NormalDialogue nd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialogueManager.instance.ReadSign();
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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

            if (Input.GetKey(KeyCode.C) && PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening)
            {
                DialogueManager.instance.StartTalking(nd);
                DialogueManager.instance.ClearText();
            }
        }
    }
}

class InanimateListener
{
    InanimateDialogue inanimateDialogue;
    BoxCollider2D collider2D;

    public void SetReferences(InanimateDialogue inanimateDialogue, BoxCollider2D collider2D)
    {
        this.inanimateDialogue = inanimateDialogue;
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
        inanimateDialogue.enabled = false;
        collider2D.enabled = false;
    }

    public void GameResumed()
    {
        inanimateDialogue.enabled = true;
        collider2D.enabled = true;
    }
}
