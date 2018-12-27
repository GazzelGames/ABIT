using System.Collections;
using UnityEngine;

public class DoorTriggerScript : MonoBehaviour {

    public NormalDialogue nd;
    public Transform destination;
    GameObject player;
    public AudioClip doorKnocking;
    AudioSource audioSource;
    public KeyCode key;

    public WorldState.WorldStateEnum worldState;

    private void Start()
    {
        doorKnocking = Resources.Load<AudioClip>("Audio/KnockOnDoor");
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        nd = new NormalDialogue();
        nd.dialogue = new string[1];
        nd.dialogue[0] = "(Knock, Knock, Knock.....apparently they are not answering.)";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&Input.GetKey(key)&&PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening)//this needs to contain the players direction 
        {
            if (worldState == WorldManager.instance.worldState)
            {
                DialogueManager.instance.StartTalking(nd);
            }
            else
            {                
                MoveDestination(collision.gameObject);
            }
        }
    }

    void MoveDestination(GameObject gameObj)
    {
        audioSource.volume = GameManager.instance.VolumeModifier;
        audioSource.PlayOneShot(doorKnocking);
        HudCanvas.instance.FadeInFader();
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameTransition;
        //PlayerMangerListener.instance.GameHaulted = true;
        player = gameObj;       //this gameObject should be the player
        Invoke("GoToDestination", 1);
    }

    void GoToDestination()
    {
        PlayerMangerListener.instance.LastDoor = destination.position;
        player.transform.position = destination.position;
        CameraController.instance.SetTransform(destination);
        HudCanvas.instance.FadeOutFader();
        Invoke("GivePlayerControl", 1);

    }

    void GivePlayerControl()
    {
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
        //PlayerMangerListener.instance.GameHaulted = false;
    }
    
}
