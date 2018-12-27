using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenChestScript : MonoBehaviour {

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
        if (collision.gameObject.CompareTag("Player")&&Input.GetKey(KeyCode.C))
        {
            AudioClip OpenChest = Resources.Load<AudioClip>("Audio/OpeningChest");
            AudioSource.PlayClipAtPoint(OpenChest,transform.position);
            objSprite.sprite = openChest;
            GetComponent<BoxCollider2D>().size = new Vector2(0.96f, 1.346896f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, 1.096838f);
            StartCoroutine(ItemRecieved(collision.gameObject));
            DialogueManager.instance.ClearText();
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    IEnumerator ItemRecieved(GameObject player)
    {
        yield return new WaitForSeconds(0.25f);
        AudioClip victoryJingle = Resources.Load<AudioClip>("Audio/Jingle_Achievement_00");
        AudioSource.PlayClipAtPoint(victoryJingle, transform.position);
        DialogueManager.instance.StartTalking(nd);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        GameObject chestItem = Instantiate(item, player.transform.position + new Vector3(0, 3, 0), player.transform.rotation);
        chestItem.GetComponent<Collider2D>().enabled = false;
        yield return new WaitUntil(() => PlayerMangerListener.instance.StateOf==GameState.StateOfGame.GameListening);
        chestItem.GetComponent<Collider2D>().enabled = true;
        chestItem.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        yield return null;
    }
}
