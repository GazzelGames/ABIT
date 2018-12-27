using System.Collections;
using UnityEngine;

public class GovenorDialogueHandler : MonoBehaviour {

#pragma warning disable 649
    //private GameObject genericDialogueObj;
#pragma warning restore 649
    public GameObject guardR;
    public GameObject guardL;
    private GameObject patrik;

#pragma warning disable 649
    [SerializeField]
    NormalDialogue eatingText;
    [SerializeField]
    NormalDialogue guardAnnouncement;
    [SerializeField]
    NormalDialogue awkwardGreeting;
    [SerializeField]
    NormalDialogue guardReport;
    [SerializeField]
    NormalDialogue patrikFinal;
#pragma warning restore 649

    private Animator anim;

    // Use this for initialization
    void Start () {
        patrik = transform.parent.gameObject;
        anim = GetComponentInParent<Animator>();
        anim.SetFloat("Emotions", 0);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerDialogueTransmitter.instance.objectReference == null)
        {
            PlayerMangerListener.instance.HasControl = false;
            PlayerMangerListener.instance.InScene = true;
            //Camera follow player is false
            CameraController.instance.followPlayer = false;

            CameraController.instance.targetReached = false;
            //move camera to govenor and then talk
            CameraController.instance.variablePos = patrik.transform.position;

            PlayerDialogueTransmitter.instance.objectReference = gameObject;

            StartCoroutine(GovenorAlertedScene());
            //then start the corution that will handle all of the dialogue
        }
    }

    IEnumerator GovenorAlertedScene()
    {

        yield return new WaitUntil(() => CameraController.instance.targetReached);
        DialogueManager.instance.StartTalking(eatingText);
        yield return new WaitUntil(()=>DialogueManager.instance.enabled == false);

        //move Camera to Guard Right and then talk
        CameraController.instance.targetReached = false;
        CameraController.instance.variablePos = guardR.transform.position;
        yield return new WaitUntil(()=>CameraController.instance.targetReached);
        anim.SetFloat("Emotions", 1);

        DialogueManager.instance.StartTalking(guardAnnouncement);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        //move back to govenor for reply
        CameraController.instance.targetReached = false;
        CameraController.instance.variablePos = patrik.transform.position;
        yield return new WaitUntil(() => CameraController.instance.targetReached);
        anim.SetFloat("Emotions", 2);   //Talking emotion

        DialogueManager.instance.StartTalking(awkwardGreeting);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);
        anim.SetFloat("Emotions", 0);

        //The guard comes to report the zombies
        CameraController.instance.targetReached = false;
        CameraController.instance.variablePos = guardL.transform.position;
        yield return new WaitUntil(() => CameraController.instance.targetReached);

        DialogueManager.instance.StartTalking(guardReport);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        //The Govenor then states do something about the zombie menace
        CameraController.instance.targetReached = false;
        CameraController.instance.variablePos = patrik.transform.position;
        yield return new WaitUntil(() => CameraController.instance.targetReached);

        PlayerMangerListener.instance.InScene = false;
        anim.SetFloat("Emotions", 3);
        DialogueManager.instance.StartTalking(patrikFinal);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);
        anim.SetFloat("Emotions", 0);

        CameraController.instance.followPlayer = true;
        CameraController.instance.targetReached = false;
        PlayerMangerListener.instance.HasControl = true;

        WorldManager.instance.worldState = WorldState.WorldStateEnum.zombieAttack;
        yield return null;
    }
}
