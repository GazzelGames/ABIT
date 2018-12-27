using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellAquiredChest : MonoBehaviour {

    public NormalDialogue nd;

    public Sprite openChest;
    SpriteRenderer objSprite;

    public GameObject buttonReference;

#pragma warning disable 649
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
            objSprite.sprite = openChest;
            GetComponent<BoxCollider2D>().size = new Vector2(0.96f, 1.346896f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, 1.096838f);
            StartCoroutine(VictoryPose(collision.gameObject));
            GetComponent<CapsuleCollider2D>().enabled = false;
            DialogueManager.instance.ClearText();
        }
    }

    public int itemInteger;
    IEnumerator VictoryPose(GameObject player)
    {
        AudioClip victoryJingle = Resources.Load<AudioClip>("Audio/Jingle_Achievement_00");
        AudioSource.PlayClipAtPoint(victoryJingle, transform.position);
        PauseMenuManager.instance.AddItem(itemInteger);
        DialogueManager.instance.StartTalking(nd);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        GameObject chestItem = Instantiate(item, player.transform.position + new Vector3(0, 3, 0), player.transform.rotation);
        yield return new WaitUntil(() => PlayerMangerListener.instance.StateOf == GameState.StateOfGame.GameListening);
        //yield return new WaitUntil(() => !PlayerMangerListener.instance.Talking);
        Destroy(chestItem);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        buttonReference.GetComponent<Button>().enabled = true;
        buttonReference.GetComponent<Image>().enabled = true;
        yield return null;
    }
}