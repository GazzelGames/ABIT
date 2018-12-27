using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SmallKeyChest : MonoBehaviour {

    public Sprite openChest;
    SpriteRenderer objSprite;

#pragma warning disable 649
    [SerializeField]
    NormalDialogue nd;

    [SerializeField]
    GameObject item;
#pragma warning restore 649

    private void Start()
    {
        objSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.DisplayToOpen();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.ClearText();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.C))
        {
            OpenChest();
            StartCoroutine(ItemRecieved(collision.gameObject));
            DialogueManager.instance.ClearText();
        }
    }

    public void OpenChest()
    {
        objSprite.sprite = openChest;
        GetComponent<BoxCollider2D>().size = new Vector2(0.96f, 1.346896f);
        GetComponent<BoxCollider2D>().offset = new Vector2(0, 1.096838f);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    IEnumerator ItemRecieved(GameObject player)
    {
        DialogueManager.instance.StartTalking(nd);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        //PlayerMangerListener.instance.Talking = true;
        GameObject chestItem = Instantiate(item, player.transform.position + new Vector3(0, 3, 0), player.transform.rotation);
        chestItem.GetComponent<Collider2D>().enabled = false;
        yield return new WaitUntil(() => PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening);
        //yield return new WaitUntil(() => !PlayerMangerListener.instance.Talking);
        chestItem.GetComponent<Collider2D>().enabled = true;
        chestItem.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        //PlayerMangerListener.instance.Talking = false;
        yield return null;
    }
}
