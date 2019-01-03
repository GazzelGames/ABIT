using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancerDialogue : MonoBehaviour {

    public NormalDialogue intro;
    public NormalDialogue alteredIntro;

    public NormalDialogue endOfFirstDialogue;

    public Vector3 movePlayerPoint;
    public bool introOver;

    private void OnEnable()
    {
        GetComponent<Animator>().SetBool("IsMoving", false);
    }

    // Use this for initialization
    void Start()
    {
        portal = transform.parent.parent.gameObject.transform.GetChild(0).gameObject;
        PlayerMangerListener.PlayerDead += ResetDialogue;
        NecroMancerManager.EndFirstBattle += EndofFirstEncounter;
        movePlayerPoint = new Vector3(transform.position.x, transform.position.y - 4f, 0);
    }

    private void OnApplicationQuit()
    {
        PlayerMangerListener.PlayerDead -= ResetDialogue;
        NecroMancerManager.EndFirstBattle -= EndofFirstEncounter;
    }

    void ResetDialogue()
    {
        OnApplicationQuit();
        //Invoke("Restore", 1.5f);
    }

    void Restore()
    {
        this.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        NecroMancerManager.instance.FirstEncounter = false;
        introOver = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening)
        {
            PlayerMangerListener.instance.HasControl = false;
            CameraController.instance.GoToTransformPosition(transform.position);
            StartCoroutine(StartIntroSpeech());
            //EndofFirstEncounter();
        }
    }

    IEnumerator StartIntroSpeech()
    {
        MusicManager.instance.AreaTag = "NecroIntro";

        GetComponent<BoxCollider2D>().enabled = false;
        //Destroy(GetComponent<BoxCollider2D>());
        yield return new WaitUntil(() => CameraController.instance.targetReached);

        if (NecroMancerManager.instance.NecroMetPlayer)
        {
            DialogueManager.instance.StartTalking(alteredIntro);
        }
        else {
            DialogueManager.instance.StartTalking(intro);
        }

        yield return new WaitUntil(() => PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening);
        CameraController.instance.followPlayer = true;
        NecroMancerManager.instance.FirstEncounter = true;
        PlayerMangerListener.instance.HasControl = true;
        introOver = true;

        MusicManager.instance.AreaTag = "NecroBattle";
        Invoke("StopCoroutines", 0.25f);
        yield return null;

        //GetComponent<NecroMancerDialogue>().enabled = false;
    }

    void StopCoroutines()
    {
        StopAllCoroutines();
    }

    void EndofFirstEncounter()
    {
        MusicManager.instance.AreaTag = "EndOfBattle";
        PlayerMangerListener.instance.HasControl = false;
        CameraController.instance.GoToTransformPosition(transform.position);
        StartCoroutine(IntroEndSpeech());
    }

    public GameObject portal;
    IEnumerator IntroEndSpeech()
    {
        yield return new WaitUntil(() => CameraController.instance.targetReached);
        DialogueManager.instance.StartTalking(endOfFirstDialogue);

        yield return new WaitUntil(() => !DialogueManager.instance.enabled);
        CameraController.instance.followPlayer = true;

        PlayerMangerListener.instance.HasControl = false;

        portal.SetActive(true);
        GetComponent<Animator>().SetTrigger("NecroHit");
        CameraController.instance.GoToTransformPosition(portal.transform.position);
        yield return null;
    }
}
