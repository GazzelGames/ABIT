using UnityEngine;
using System.Collections;

public class GovenorDoor : MonoBehaviour {

    public NormalDialogue nd;
    public Transform destination;
    public KeyCode key;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !PlayerMangerListener.instance.HasSword)
        {
            DialogueManager.instance.StartTalking(nd);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(key) && (HudCanvas.instance.spriteFader.color.a == 0) && PlayerMangerListener.instance.HasSword&&PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening)//this needs to contain the players direction 
        {
            MoveDestination(collision.gameObject);
        }
    }

    void MoveDestination(GameObject gameObj)
    {
        HudCanvas.instance.FadeInFader();
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameTransition;
        //PlayerMangerListener.instance.GameHaulted = true;
        StartCoroutine(Arrived(gameObj));
    }

    IEnumerator Arrived(GameObject @object)
    {
        yield return new WaitForSeconds(1f);
        @object.transform.position = destination.position;
        CameraController.instance.SetTransform(destination);
        HudCanvas.instance.FadeOutFader();
        yield return new WaitUntil(() => (HudCanvas.instance.spriteFader.color.a == 0));
        PlayerMangerListener.instance.StateOf = GameState.StateOfGame.GameListening;
        //PlayerMangerListener.instance.GameHaulted = false;
        yield return null;
    }




}
