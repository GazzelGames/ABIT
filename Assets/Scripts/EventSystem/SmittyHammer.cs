using System.Collections;
using UnityEngine;

public class SmittyHammer : MonoBehaviour {

    public QuestItem questHammer;  //this equipment script is attached to the gameobject
    public NormalDialogue[] nd;
    public NormalDialogue nothingHappend;

    public GameObject ironSword; //this reference is for the iron sword gameobject
    private GameObject blacksmith;  // this is a reference to the black smith object
    private GameObject player;  // this reference is the player object

    private void Start()
    {
        PlayerMangerListener.instance.HasControl = false;
        player = GameObject.Find("Player");
        blacksmith = GameObject.Find("BlackSmith");
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        player.GetComponent<Animator>().SetBool("VictoryPose", true);

        try
        {
            if ((transform.position - blacksmith.transform.position).magnitude < 6.5f)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("DisableRender", 1); //return player back to normal stance with the this invoke
                StartCoroutine(NewHammerScene());
                SetTalkingStance();
                if (blacksmith.transform.GetChild(0).gameObject.GetComponent<BlackSmithDialogue>())
                {
                    blacksmith.transform.GetChild(0).gameObject.GetComponent<BlackSmithDialogue>().hammerRecieved = true;
                    blacksmith.transform.GetChild(0).gameObject.GetComponent<BlackSmithDialogue>().waitingForHammer = false;
                }
            }
            else
            {
                StartCoroutine(NothingThere());
            }
        }
        catch
        {
            StartCoroutine(NothingThere());
        }
    }

    void DisableRender()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator NewHammerScene()
    {
        PlayerMangerListener.instance.InScene = true;

        DialogueManager.instance.StartTalking(nd[0]);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        GameObject sword = Instantiate(ironSword, player.transform.position + new Vector3(0, 3, 0),transform.rotation);   //spawns the sword about the player
        player.GetComponent<Animator>().SetBool("VictoryPose", true);
        print("play victory music then make coroutine wait until its finished");
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        sword.transform.position = player.transform.position;

        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        DialogueManager.instance.StartTalking(nd[1]);
        PlayerMangerListener.instance.InScene = false;
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);

        PauseMenuManager.instance.EndQuestItem(questHammer);
        Destroy(gameObject, 0.5f);
        PlayerMangerListener.instance.HasControl = true;
        yield return null;
    }

    IEnumerator NothingThere()
    {
        yield return new WaitForSeconds(1); //this will be played by a audio clip
        DialogueManager.instance.StartTalking(nothingHappend);
        yield return new WaitUntil(() => DialogueManager.instance.enabled == false);
        //yield return DialogueManager.instance.SceneDialogue(nothingHappend);
        player.GetComponent<Animator>().SetBool("VictoryPose", false);
        Destroy(gameObject);
        PlayerMangerListener.instance.HasControl = true;
        yield return null;
    }

    public void SetTalkingStance()
    {
        Vector3 displacement = blacksmith.transform.position - player.transform.position;
        float x = displacement.x;
        float y = displacement.y;
        float stance = 0;
        float playerStance = 0;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
            {
                stance = 1;
                playerStance = 2;
            }
            else
            {
                stance = 2;
                playerStance = 1;
            }
        }
        else
        {
            if (y > 0)
            {
                stance = 0;
                playerStance = 3;
            }
            else
            {
                stance = 3;
                playerStance = 0;
            }
        }
        player.GetComponent<Animator>().SetFloat("IdleStance", playerStance);
        blacksmith.GetComponent<Animator>().SetFloat("IdleStance", stance);
    }
}
