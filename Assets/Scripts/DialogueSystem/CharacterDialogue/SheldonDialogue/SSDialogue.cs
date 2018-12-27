using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDialogue : MonoBehaviour {

    public int element;
    public NormalDialogue[] scenarios;
    SkeletonSheldonListener sheldonDialogueListener;
    Animator parentAnim;
    bool canSubscribe;

    private void Awake()
    {
        sheldonDialogueListener = new SkeletonSheldonListener();
        sheldonDialogueListener.SetReferences(this, GetComponent<BoxCollider2D>());
        canSubscribe = true;
    }

    private void OnEnable()
    {
        if (canSubscribe)
        {
            sheldonDialogueListener.Subscribe();
            canSubscribe = false;
        }
    }

    private void OnDisable()
    {
        if (gameObject.activeInHierarchy == false)
        {
            sheldonDialogueListener.Unsubscribe();
            canSubscribe = true;
        }

    }

    public void Start()
    {
        parentAnim = GetComponentInParent<Animator>();
        element = 0;
    }

    void GetDialogue()
    {
        PlayerMangerListener.instance.InScene = true;
        switch (element)
        {           
            case 0:
                {
                    StartCoroutine(Scenario1());
                    element = 1;
                    break;
                }
            case 1:
                {
                    StartCoroutine(Scenario2());
                    element = 2;
                    break;
                }
            case 2:
                {
                    StartCoroutine(Scenario3());
                    element = 0;
                    break;
                }
        }
    }

    IEnumerator Scenario1()
    {
        parentAnim.SetTrigger("PutHatOn");
        DialogueManager.instance.StartTalking(scenarios[0]);

        //I need something here to work
        yield return new WaitUntil(() =>!DialogueManager.instance.isActiveAndEnabled);

        parentAnim.SetTrigger("Talk");
        DialogueManager.instance.StartTalking(scenarios[1]);

        PlayerMangerListener.instance.InScene = false;

        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);

        parentAnim.SetTrigger("TakeHatOff");
        yield return new WaitForSeconds(0.2f);
        yield return null;
    }

    IEnumerator Scenario2()
    {

        parentAnim.SetTrigger("PutHatOn");
        DialogueManager.instance.StartTalking(scenarios[2]);

        //I need something here to work
        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);

        parentAnim.SetTrigger("Talk");
        parentAnim.SetTrigger("StepForward");
        DialogueManager.instance.StartTalking(scenarios[3]);

        PlayerMangerListener.instance.InScene = false;

        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);

        parentAnim.SetTrigger("TakeHatOff");
        yield return new WaitForSeconds(0.2f);
        yield return null;
    }

    IEnumerator Scenario3()
    {
        parentAnim.SetTrigger("PutHatOn");
        DialogueManager.instance.StartTalking(scenarios[4]);

        //I need something here to work
        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);

        parentAnim.SetTrigger("Talk");
        parentAnim.SetTrigger("StepForward");
        DialogueManager.instance.StartTalking(scenarios[5]);

        PlayerMangerListener.instance.InScene = false;

        yield return new WaitUntil(() => !DialogueManager.instance.isActiveAndEnabled);

        PlayerMangerListener.instance.HasControl = false;
        parentAnim.SetTrigger("TakeHatOff");
        yield return new WaitForSeconds(1f);
        PlayerMangerListener.instance.HasControl = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            GetComponent<SSDialogue>().enabled = true;
            PlayerDialogueTransmitter.instance.objectReference = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        GetComponent<SSDialogue>().enabled = false;
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
                DialogueManager.instance.ClearText();
            }
        }

    }


}

class SkeletonSheldonListener
{
    SSDialogue sSDialogue;
    BoxCollider2D collider2D;

    public void SetReferences(SSDialogue sSDialogue, BoxCollider2D collider2D)
    {
        this.sSDialogue = sSDialogue;
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
        sSDialogue.enabled = false;
        collider2D.enabled = false;
    }

    public void GameResumed()
    {
        sSDialogue.enabled = true;
        collider2D.enabled = true;
    }

}
